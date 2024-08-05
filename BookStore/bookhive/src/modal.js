import Modal from "react-bootstrap/Modal"
import Form from 'react-bootstrap/Form'
import Button from 'react-bootstrap/Button';
import { useEffect, useState } from "react";
import { getData, postData, postFormData } from "./fetch";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Link } from "react-router-dom";
const FormData = require('form-data')
export default function Model(props) {
  const [genere, setGenere] = useState("");
  const [author, setAuthor] = useState({ name: "", biography: "", author_image: "" })
  const [error, setError] = useState({});
  const [upload, setUpload] = useState(false);
  useEffect(() => {
    if (props.id && props.id > 0) {
      getAuthor(props.id)
    }
  }, [])
  async function getAuthor(id) {
    const auth = await getData(`https://localhost:7136/Author/${id}`, "get");
    setAuthor(auth);
  }
  function handleData(event) {
    if (props.type === 'genere') {
      setGenere(event.target.value);
    }
    else {
      if (event.target.name === 'author_image') {
        // console.log(event.target.files[0]);
        setAuthor({ ...author, [event.target.name]: event.target.files[0] })
      }
      else {
        setAuthor({ ...author, [event.target.name]: event.target.value });
      }
    }
  }
  async function addData() {
    if (props.type === 'genere') {
      if (genere === '' || genere === null || typeof genere === 'undefined') {
        let error = {};
        error.name = "Genere name should not be empty";
        setError(error);
      }
      else {
        let msg = await postData("https://localhost:7136/Genre", "post", { genre_name: genere });
         if (msg.hasOwnProperty('genre_id')) {
          toast.success("Added Successfully", {
            onClose: () => {props.close();props.onload();}
          });
        }
        else {

          toast.error(msg, {
          })
        }
      }
    }
    else {
      let error = {};
      if (author.name === '' || author.name === null || typeof author.name === 'undefined') {
        error.name = "Author's name should not be empty";
      }
      else if (author.biography === '' || author.biography === null || typeof author.biography === 'undefined') {
        error.biography = "Biography should not be Empty"
      }
      else {
        var data = new FormData();
        Object.entries(author).forEach(([key, value]) => {
          if (value !== '' && value !== null) {
            data.append(key, value);
          }
        });
        let msg
        if (props.id > 0) {
          msg = await postFormData(`https://localhost:7136/Author/${props.id}`, "put", data);


        }
        else {
          msg = await postFormData("https://localhost:7136/Author", "post", data);
        }

         if (msg.hasOwnProperty('author_id')) {
          let message;
          if (props.id > 0) {
            message = "Author Updated Successfully";
          }
          else {
            message = "Author Created Successfully";
          }
          toast.success(message, {
            onClose: () => { props.onload();props.close(); }
          });
        }
        else {
          toast(msg, {
            onClose: () => { props.close(); }
          });

        }
      }
      setError(error);
    }
  }
  return (
    <>
      <Modal show={props.show} onHide={props.onClick}>
        <Modal.Header>
          {props.close && <Modal.Title><h1 className="h3">{props.type === 'genere' ? "Add Genre" : props.id > 0 ? "Edit Author" : "ADD Author"}</h1></Modal.Title>}
          <span className="btn-close cur" style={{ float: "right !important" }} onClick={props.onClick ? props.onClick : props.close}></span>
        </Modal.Header>
        <Modal.Body>{props.msg ? props.msg :
          <Form>
            <Form.Control name={props.type === 'genere' ? "genre_name" : "name"} placeholder={props.type === 'genere' ? "Enter Genere Name" : "Enter Author's Name"} className={`mt-2 ${error.name ? "border border-danger" : 'border border-secondary'}`} onChange={handleData} value={props.id ? author.name : genere.genre_name} />
            {error.name && <h1 className="text-danger mt-1 h6">{error.name}</h1>}
            {props.type !== 'genere' &&
              <>
                <Form.Control name="biography" as="textarea" placeholder="Enter Author's biography" className={`mt-3 ${error.biography ? "border border-danger" : 'border border-secondary'}`} onChange={handleData} value={author.biography} />
                {error.biography && <h1 className="text-danger mt-1 h6">{error.biography}</h1>}
                {props.id > 0 ?
                  (author.author_image === null ? <h1 className="text-dark mt-2 h6">Image was not uploaded</h1> :
                    typeof author.author_image === 'object' ? ""
                      : <img src={author.author_image} alt="no" height="100" width="100" className="mt-2" />) : ""
                }
                {props.id > 0 &&
                  <Link to="#" type="btn"
                    onClick={() => setUpload(true)} className="mt-2">Upload New Image</Link>
                }
                {(typeof props.id === 'undefined' || props.id === 0 || upload === true) &&
                  <Form.Control type="file" accept="image/*" name="author_image" placeholder="Select Image" className="mt-3 border border-secondary" onChange={handleData} />}
              </>
            }

          </Form>


        }</Modal.Body>
        <Modal.Footer>
          {props.type && <Button variant="primary" onClick={props.value ? props.value : addData}>
            {props.id ? props.id > 0 ? "Edit Author" : "Add Author" : "Ok"}
          </Button>}
          <Button variant="secondary" onClick={props.onClick ? props.onClick : props.close}>
            Close
          </Button>
        </Modal.Footer>
       {props.showToast && <ToastContainer position="top-center" autoClose={1000}/>}
      </Modal>

    </>
  )
}