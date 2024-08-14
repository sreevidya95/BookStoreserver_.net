import { useEffect, useState } from "react";
import { getData } from "./fetch";
import { useParams, useNavigate } from "react-router-dom";
import { isMobile } from "react-device-detect";
import Footer from "./Footer";
import Header from "./Header";
export default function BookDetails() {
    const [loading, setloading] = useState(false);
    let { id } = useParams();
    const [book, setBook] = useState({});
    const navigate = useNavigate();
    async function getBook() {
        setloading(true);

        let b = await getData(`https://localhost:7136/Books/${id}`, "get");
        setBook(b);
        setloading(false);

    }
    useEffect(() => {
        setloading(true);
        getBook();
    }, [id]);
    function signOut() {
        localStorage.removeItem("id");
        localStorage.removeItem("name");
        localStorage.removeItem('email');
        navigate("/");
    }
    return (
        <div className="container-fluid">
            <div className="row">
                <Header onClick={() => signOut()} />
            </div>
            {book && loading ?
                <div className="row">
                    <div className="spinner-grow sp col-6 offset-6" role="status">
                        <span className="sr-only">Loading...</span>
                    </div>
                </div> :
                <div className="row">
                    <div className="col-12 col-md-6 col-xl-6 mt-5 book">
                        {book.book_image ? <img src={"data:image/jpeg;base64,"+book.book_image} alt="no" width={!isMobile && "500"} className="b-img" /> : <img src="/noimg.webp" alt="no" width={!isMobile && "500"} className="b-img" />}
                    </div>
                    <div className="col-12 col-md-6 col-xl-6 mt-5 auth">
                        {book.author && <h6 className="fs-2 text-secondary col-12 arsenal-sc-regular">By {book.author.name}</h6>}
                        <h1 className="display-4 arsenal-sc-regular">{book.title}</h1>
                        <div className="col-mt-3 fs-1 col-12 arsenal-sc-regular">${book.price}</div>
                        <p className="col-12 fs-5 text-secondary">{book.title} Dive into a captivating narrative where compelling characters navigate a world fraught with
                            challenges and discoveries. This tale weaves together elements of suspense, adventure,
                            and poignant reflections, inviting readers on a journey that explores themes of identity,
                            courage, and the intricacies of human relationships. With vivid prose and masterful storytelling,
                            {book.author && book.author.name} crafts a rich tapestry that will leave readers eagerly turning each page, eager to uncover the secrets that lie beneath
                            the surface</p>
                        {book.genre && <h6 className="fs-3 text-dark col-12 arsenal-sc-regular">Category: {book.genre.genre_name}</h6>}
                        {book.publication_date && <h6 className="fs-5 text-dark col-12 arsenal-sc-regular">Plublcation Date: {book.publication_date}</h6>}
                        <hr />
                        {book.author &&
                            <div className="row">
                                <div className="col-md-3 col-xl-3 col-12 mt-5 book">
                                    {book.author && book.author.author_image ? <img src={"data:image/jpeg;base64,"+book.author.author_image} alt="no" width="100" height="100" className="auth_img" /> :
                                        <img src="/user.jpg" alt="no" width="100" className="auth_img" />}
                                </div>
                                <div className="col-md-8 col-xl-8 col-12 mt-5">
                                    {book.author && <h6 className="fs-2 col-12 arsenal-sc-regular text-dark">{book.author.name}</h6>}
                                    {book.author && <h6 className="fs-6 text-secondary col-12">{book.author.biography}</h6>}
                                </div>
                            </div>
                        }
                    </div>
                </div>}
            <footer className='mt-5'>
                <Footer></Footer>
            </footer>
        </div>
    );
}