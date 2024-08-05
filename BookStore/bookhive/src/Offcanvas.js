import { useEffect, useRef, useState } from "react";
import { Offcanvas } from "react-bootstrap";
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button';
import Col from 'react-bootstrap/Col';
import { getData, postFormData, postData } from "./fetch";
import Model from "./modal";
import { Link } from "react-router-dom";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
const FormData = require('form-data')
export default function Offcanva(props) {
  const [authors, setAuthors] = useState();
  const [generes, setGenere] = useState();
  const [loading, setloading] = useState(false);
  const [to, setToast] = useState(false);
  const [add, setAdd] = useState("");
  const [error, setError] = useState({});
  const [eff, setEff] = useState(false);
  const [upload, setUpload] = useState(false);
  const [offer, setOffer] = useState({ name: "", discount: "", startDate: "", endDate: "", book: [] })
  const [book, setBook] = useState({ title: "", price: "", publication_date: "", book_image: "", authorAuthorId: "", genreGenreId: "" })
  const id = useRef(0);
  useEffect(() => {
    if (typeof props.id !== 'undefined' || props.book)
      LoadData();
  }, [eff]);
  function handleClose() {
    setToast(false);
  }
  function load(){
    if(eff){
        setEff(false)
    }
    else{
        setEff(true)
    }
}
  async function LoadData() {
    setloading(true);
    if (props.id > 0) {
      const b = await getData(`https://localhost:7136/Books/${props.id}`, "get");
      id.current = b.offerOfferId;
      //const off = await getData("http://localhost:3000/offer", "get");

      setBook(b);
      //setOffer(off)
    }
    if (props.book) {
      const b = await getData(`https://localhost:7136/Books`, "get");
      setBook(b);
    }
    const author = await getData("https://localhost:7136/Author", "get");
    const genere = await getData("https://localhost:7136/Genre", "get");
    setGenere(genere);
    setAuthors(author);
    setloading(false);
  }
  const [type, setType] = useState("text");
  async function AddBook(id) {
    let error = {};
    if (book.title === '') {
      error.title = "Book's Title Shouldnt be Empty";
    }
    else if (book.price === '') {
      error.price = "Book's Price Shouldnt be Empty";
    }
    else if (book.publication_date === '') {
      error.pd = "Book's Publication Date Shouldnt be Empty";
    }
    else if (book.authorAuthorId === '') {
      error.aid = "Please Select Author of the Book";
    }
    else if (book.genreGenreId === '') {
      error.gid = "Please Select Book's Genere";
    }
    else {
      setloading(true)
      var data = new FormData();
      Object.entries(book).forEach(([key, value]) => {
        if (value !== '' && value !== null) {
          data.set(key, value);
        }
      });
      let msg;
      if (props.id > 0) {
        delete book.author;
        delete book.genre;
        msg = await postFormData(`https://localhost:7136/Books/${props.id}`, "put", data);
      }
      else {
        msg = await postFormData("https://localhost:7136/Books/", "post", data);
      }
       if (msg.hasOwnProperty('book_id')) {
        let message
        if (props.id > 0) {
          message = "Book Updated Successfully";
        }
        else {
          message = "Book Added Successfully";
        }
        toast.success(message, {
          onClose: () => { props.onload(); props.onClick(); }
        });
      }
      else {
        toast("Something went wrong", {
          onClose: () => props.onClick()
        });

      }
      setloading(false)
    }
    setError(error);
  }
  async function removeSale(id) {
    let msg = await postData(`http://localhost:3000/books/sale/${id}`, "put", { offerOfferId: null });
    toast(msg.msg, {
      onClose: () => { props.onload() }
    })
  }
  function dateFocus(e) {
    e.target.type = "date"
  }
  async function AddAdmin() {
    setloading(true);
    let error = {};
    if (offer.name === '') {
      error.name = "Sale Name should not be Empty";
    }
    else if (offer.discount === '') {
      error.discount = "Enter Valid Dicount"
    }
    else if (offer.startDate === '') {
      error.sd = "Enter Start Date of Offer"
    }
    else if (new Date(offer.startDate).setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
      error.sd = "Start Date's minimum date starts from today";
    }
    else if (offer.endDate === '') {
      error.ed = "Enter End Date"
    }
    else if (new Date(offer.endDate).setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
      error.ed = "End Date's minimum date starts from today";
    }
    else if (offer.book === '' || offer.book.length === 0) {
      error.book = "Select Atleast One Book"
    }
    else if (offer.startDate >= offer.endDate) {
      error.sd = "Start Should be less than End Date"
    }
    else {
      let msg = await postData("http://localhost:3000/offer", "post", offer);
      if (msg.hasOwnProperty('msg')) {
        toast.error(msg.msg,
        );

      }
      else if (msg.hasOwnProperty('offer_id')) {
        toast.success("Offer Added Successfully", {
          onClose: () => { props.onClick(); window.location.reload(); }
        })
      }
    }
    setError(error);
    setloading(false)
  }
  function handleChange(event) {
    if (event.target.name === 'book_image') {
      setBook({ ...book, [event.target.name]: event.target.files[0] });
    }
    else {
      if (typeof props.id !== 'undefined') {
        setBook({ ...book, [event.target.name]: event.target.value });
      }
      else {
        if (event.target.name === 'book') {
          let value = Array.from(event.target.selectedOptions, option => option.value);
          setOffer({ ...offer, [event.target.name]: value });
        }
        else {
          setOffer({ ...offer, [event.target.name]: event.target.value });
        }
      }
    }
  }
  return (
    <Offcanvas show={props.show} placement="end">
      <Offcanvas.Header>
        <Offcanvas.Title>
          <h3 className="h3">{(typeof props.id !== 'undefined') ? props.id > 0 ? "Edit Book" : "Add Book" : "Add Sale"}</h3>
        </Offcanvas.Title>
        <span className="btn-close cur" style={{ float: "right !important" }} onClick={props.onClick}></span>
      </Offcanvas.Header>
      <Offcanvas.Body>
        <div className="container">
          <div className="row">
            {loading ?
              <div className="row">
                <div className="spinner-grow sp col-6 offset-6" role="status">
                  <span className="sr-only">Loading...</span>
                </div>
              </div>
              :
              <Form className="mt-5">
                {(typeof props.id !== 'undefined') ?
                  <>
                    <Form.Control name="title" placeholder="Enter Book Name" className={`mt-2 ${error.title ? "border border-danger" : "border border-secondary"}`} onChange={handleChange} value={book.title} />

                    {error.title && <h1 className="text-danger mt-2 h6">{error.title}</h1>}

                    <Form.Control name="price" type="number" placeholder="Enter Price of book" className={`mt-3 ${error.price ? "border border-danger" : "border border-secondary"}`} onChange={handleChange}

                      min="1" value={book.price} step="0.10" />

                    {error.price && <h1 className="text-danger mt-2 h6">{error.price}</h1>}

                    <Form.Control name="publication_date" type={type} placeholder="Enter Publication Date"
                      onFocus={() => setType("date")} className={`mt-3 ${error.pd ? "border border-danger" : "border border-secondary"}`} onChange={handleChange} value={book.publication_date} />

                    {error.pd && <h1 className="text-danger mt-2 h6">{error.pd}</h1>}
                    {props.id > 0 ?
                      (book.book_image === null || book.book_image==='' ? <h1 className="text-dark mt-2 h6">Image was not uploaded</h1> :
                        typeof book.book_image === 'object' ? ""
                          : <img src={"data:image/jpeg;base64," + book.book_image} alt="no" height="100" width="100" className="mt-2" />) : ""
                    }
                    {props.id > 0 &&
                      <Link to="#" type="btn"
                        onClick={() => setUpload(true)} className="mt-2">Upload New Image</Link>
                    }
                    {(props.id === 0 || upload === true) &&
                      <Form.Control type="file" accept="image/*" name="book_image" placeholder="Select Image" className="mt-3 border-secondary" onChange={handleChange} />}

                    <Form.Select name="authorAuthorId" className={`mt-3 ${error.aid ? "border border-danger" : "border border-secondary"}`} onChange={handleChange} value={book.authorAuthorId}>
                      <option value="">Select Author</option>
                      {authors && authors.map(e =>
                        <option value={e.author_id} key={e.author_id}>{e.name}</option>
                      )}
                    </Form.Select>

                    {error.aid && <h1 className="text-danger mt-2 h6">{error.aid}</h1>}

                    <p className="mt-3">Couldn't find the Author You Want ? <Link to="#" type="btn"
                      onClick={() => { setToast(true); setAdd("addAuthor") }}> Click Here</Link></p>

                    <Form.Select name="genreGenreId" onChange={handleChange} value={book.genreGenreId} className={`mt-3 ${error.gid ? "border border-danger" : "border border-secondary"}`}>
                      <option value="">Select Genre</option>
                      {generes && generes.map(e =>
                        <option value={e.genre_id} key={e.genre_id}>{e.genre_name}</option>
                      )}
                    </Form.Select>
                    <p className="mt-3">Couldn't find the Genre You Want ? <Link to="#" type="btn"
                      onClick={() => { setToast(true); setAdd("genere") }}> Click Here</Link></p>
                    {error.gid && <h1 className="text-danger mt-2 h6">{error.gid}</h1>}
                    {props.id > 0 && offer.length > 0 && <Form.Select name="offerOfferId" onChange={handleChange} value={book.offerOfferId} className={`mt-3 ${error.gid ? "border border-danger" : "border border-secondary"}`}>
                      <option value="">Apply Offers</option>
                      {offer.length > 0 && offer.map(e =>
                        <option value={e.offer_id} key={e.offer_id}>{e.name}</option>
                      )}
                    </Form.Select>}
                    {props.id > 0 && book && book.offerOfferId && id.current > 0 && <p className="mt-3">Want to remove Sale on this Book? <Link to="#" type="btn"
                      onClick={() => { removeSale(book.book_id) }}> Click Here</Link></p>}
                    <Button type="submit" as={Col} xs={{ span: 4, offset: 4 }} className="mt-5" onClick={() => AddBook(props.id)}>{props.id > 0 ? "Edit Book" : "Add Book"}</Button>
                  </> :
                  <>
                    <Form.Control name="name" placeholder="Enter Sale Name" className={`mt-3 ${error.name ? "border border-danger" : "border border-secondary"}`} onChange={handleChange} />
                    {error.name && <h1 className="text-danger mt-2 h6">{error.name}</h1>}
                    <Form.Control type="number" name="discount" placeholder="Enter Discount percentage" min="1" max="100" className={`mt-3 ${error.discount ? "border border-danger" : "border border-secondary"}`} onChange={handleChange} required />
                    {error.discount && <h1 className="text-danger mt-2 h6">{error.discount}</h1>}
                    <Form.Control type="text" name="startDate" placeholder="Enter Start Date"
                      className={`mt-3 ${error.sd ? "border border-danger" : "border border-secondary"}`} onFocus={dateFocus} onChange={handleChange} required />
                    {error.sd && <h1 className="text-danger mt-2 h6">{error.sd}</h1>}
                    <Form.Control type="text" name="endDate" placeholder="Enter End Date" onFocus={dateFocus} className={`mt-3 ${error.ed ? "border border-danger" : "border border-secondary"}`} onChange={handleChange} required />
                    {error.ed && <h1 className="text-danger mt-2 h6">{error.ed}</h1>}
                    <Form.Control as="select" name="book" onChange={handleChange} value={book.book_id} className={`mt-3 ${error.book ? "border border-danger" : "border border-secondary"}`} multiple>
                      <option value="" className="fw-bold">Select Books</option>
                      {book.length > 0 && book.map(e =>
                        <option key={e.book_id} value={e.book_id}>{e.title}</option>
                      )}
                    </Form.Control>
                    {error.book && <h1 className="text-danger mt-2 h6">{error.book}</h1>}
                    <Button type="submit" as={Col} xs={{ span: 4, offset: 4 }} className="mt-5" onClick={() => AddAdmin()}>Submit</Button>
                    <h1 className="text-secondary mt-5" style={{ fontSize: "12px" }}>* Sale on books will be removed automatically after offer ends *</h1>
                  </>
                }
              </Form>}
            {to && <Model show={toast} onClick={handleClose} type={add} close={handleClose} onload={()=>load()}/>}
          </div>
        </div>
      </Offcanvas.Body>
      <ToastContainer position="top-center" autoClose={1000}/>
    </Offcanvas>
  )
}