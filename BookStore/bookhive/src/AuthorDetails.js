import { useEffect, useState } from "react";
import Header from "./Header";
import { useNavigate, useParams } from "react-router-dom";
import { getData } from "./fetch";
import Footer from "./Footer";
import { isMobile } from "react-device-detect";
export default function AuthorDetails() {
    const navigate = useNavigate();
    const { id } = useParams();
    const [loading, setloading] = useState(false);
    const [data, setData] = useState();
    const [books, setBooks] = useState();
    function signOut() {
        localStorage.removeItem("id");
        localStorage.removeItem("name");
        localStorage.removeItem('email');
        navigate("/");
    }
    useEffect(() => {
        loadAuthor(id);
    }, [id]);
    async function loadAuthor(i) {
        setloading(true)
        const author = await getData(`http://localhost:3000/authors/${i}`, "get");
        if (author) {
            setData(author);
            const bookdata = await getData(`http://localhost:3000/books/author/${i}`, "get");
            setBooks(bookdata);
        }
        else {
            alert("something went wrong")
        }
        setloading(false)
    }
    return (
        <div className="container-fluid">
            <div className="row">
                <Header onClick={() => signOut()} />
            </div>
            {loading ?
                <div className="row">
                    <div className="spinner-grow sp col-6 offset-6" role="status">
                        <span className="sr-only">Loading...</span>
                    </div>
                </div> :
                <>
                    <div className="row">
                        <div className="mt-5 ap">
                            <span className="col-12">
                                <h5 className="display-4 text-dark ps-5 arsenal-sc-regular">Author</h5></span>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col-md-4 col-xl-4 col-12 mt-5">
                            {data && (data.author_image ? <img src={data.author_image} alt="no" height="300" width="300" className="ms-5 auth_img fr name" /> : <img src="/user.jpg" alt="no" height="300" width="300" className="ms-5 auth_img fr name" />)}
                        </div>
                        <div className="col-md-6 col-xl-6 col-12 mt-5 offset-xl-1 offset-md-1 auth">
                            {data && <h3 className="display-4 col-12 text-dark arsenal-sc-regular">{data.name}</h3>}
                            {data && <h3 className="h5 col-12 text-secondary mt-3">{data.biography}</h3>}
                            <h3 className="h5 col-12 text-secondary mt-3 text-dark cur">Know More about Author:
                                <i class="fa-brands fa-wikipedia-w ms-1 h3" ></i>
                            </h3>
                        </div>
                    </div>
                    <hr />
                    <div className="row">
                        <p className="col-12 h1 arsenal-sc-regular ms-5">Books</p>
                        <div className="row">
                            {books && (books.length > 0 ? books.map(e =>
                                <>
                                    {e.book_image ? <img src={e.book_image} alt="none" height="400" className={`${!isMobile && 'ms-5'} mt-5 mb-5 col-md-3 col-xl-3 col-12 book`} /> :
                                        <span className={`${!isMobile ? 'ms-5' : 'ms-3'} mt-5 mb-5 col-md-3 col-xl-3 col-12 h1 fw-bold title book`}>{e.title}</span>}
                                </>
                            ) : <h1 className="h3 text-dark text-center mb-5">No Books found for this Author</h1>)}
                        </div>
                        <footer className='mt-5'>
                            <Footer></Footer>
                        </footer>
                    </div>
                </>}
        </div>
    );
}