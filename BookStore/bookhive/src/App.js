import './App.css';
import Nav from 'react-bootstrap/Nav';
import Footer from './Footer';
import { Link, useNavigate } from 'react-router-dom';
import { Tooltip } from 'react-tooltip'
import { useEffect } from 'react';
function App() {
  const navigate = useNavigate();
  useEffect(() => {
    if (localStorage.getItem("id")) {
      navigate("/books");
    }
  }, []);
  return (
    <div className='App'>
      <div className="container-fluid">
        <header>
          <div className="row bg-dark">
            <p className='col-11 text-light'>
              Free shipping for all orders above $20
            </p>
            <div className='col-1'>
              <span className='col-1 me-3'><i className="fa-brands fa-x-twitter text-light"></i></span>
              <span className='col-1 me-3'><i className="fa-brands fa-square-instagram text-light"></i></span>
              <span className='col-1 me-3'><i className="fa-brands fa-facebook text-light"></i></span>
            </div>
          </div>
          <div className='row'>
            <nav className='navbar navbar-expand-lg col-md-12 col-sm-12'>
              <Nav.Link href="#" className='navbar-brand col-3'>
                <img className='col-3 ms-2' src="/logo.png" alt="no-image" />
                <span className='dancing-script-name col-5 fst-italic col-2'> Book Hive</span>
              </Nav.Link>
              <button className='navbar-toggler col-4' data-bs-toggle="collapse" data-bs-target="#navigation" aria-expanded="false" >
                <span class="navbar-toggler-icon"></span>
              </button>
              <div className='collpase navbar-collapse w3-animate-left' id="navigation">
                <ul className='navbar-nav mr-auto'>
                  <li className='nav-item fs-4'>
                    <Nav.Link href="#" className="text-danger disabled arsenal-sc-regular ">Home</Nav.Link>
                  </li>
                  <li className='nav-item fs-4'>
                    <Nav.Link href="/about" className='arsenal-sc-regular '>About</Nav.Link>
                  </li>
                  <li className='nav-item fs-4'>
                    <Nav.Link href="https://www.thatartsyreadergirl.com/" className='arsenal-sc-regular '>Our Blog</Nav.Link>
                  </li>
                  <li className='nav-item fs-4'>
                    <Nav.Link href="https://www.lipsum.com/" className='arsenal-sc-regular '>Events</Nav.Link>
                  </li>
                  <li className='nav-item fs-4'>
                    <Nav.Link href="https://www.lipsum.com/" className='arsenal-sc-regular '>Store</Nav.Link>
                  </li>
                </ul>
              </div>
              <Link to="/adminLogin"><span><i className="fa-solid fa-user-gear fs-3 me-3 w3-animate-right link"></i></span></Link>
              <Link to="/userLogin"><i className="fa-solid fa-user fs-3 ms-2 me-5 w3-animate-right link"></i></Link>
              <Tooltip anchorSelect='.fa-user-gear' place='bottom'>Admin Login</Tooltip>
              <Tooltip anchorSelect='.fa-user' place='bottom'>User Login</Tooltip>
            </nav>
          </div>
        </header>
        <main className='row'>
          <div id="carouselExampleCaptions" className="carousel slide" data-bs-ride="carousel">
            <div className="carousel-indicators">
              <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="0" className="active" aria-current="true" aria-label="Slide 1"></button>
              <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="1" aria-label="Slide 2"></button>
              <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="2" aria-label="Slide 3"></button>
            </div>
            <div className="carousel-inner">
              <div className="carousel-item active">
                <img src="./s.jpeg" className="d-block w-100 i" alt="..." style={{ height: "500px" }} />
                <div className="carousel-caption d-none d-md-block slide1">
                  <img src="./slide1.jpg" alt="no" />
                  <img src="./best.png" alt="no" className='bestseller' />
                  <span className='text-center slide1-content w3-animate-right'>
                    <h1 className='display-4'>The all time classic</h1>
                    <p className='fs-6 mt-2 ms-5 text-light'>
                      No.1 best seller in classic,explore more by logging in...<br />
                      and find the amazing deals to buy,explore more about author <br />
                      and the book by logging in
                    </p>
                    <button className='mt-5 text-light fs-3 col-2 rounded-3' style={{ backgroundColor: "#D14031", border: "1px solid #D14031" }}>ReadMore...</button>
                  </span>
                </div>
              </div>
              <div className="carousel-item">
                <img src="./slide2.png" className="d-block w-100" alt="..." style={{ backgroundColor: "#e8dbca" }} />
                <img src="./best.png" alt="no" className='bestseller' />
                <div className="carousel-caption d-none d-md-block">
                  <img className="eiffel" src="https://chapterone.qodeinteractive.com/wp-content/uploads/2019/07/home-2-revolution-img-2.png" height="300" width="300" alt="no" />
                  <span className='text-center slide2-content'>
                    <h1 className='display-4 text-dark'>We all love literature</h1>
                    <p className='fs-6 mt-2 ms-5 text-dark'>
                      No.1 best seller in literature,explore more by logging in...<br />
                      and find the amazing deals to buy,explore more about author <br />
                      and the book by logging in
                    </p>
                    <button className='mt-5 text-light fs-3 col-2 rounded-3' style={{ backgroundColor: "#D14031", border: "1px solid #D14031" }}>ReadMore...</button>
                  </span>
                </div>
              </div>
              <div className="carousel-item">
                <img src="https://chapterone.qodeinteractive.com/wp-content/uploads/2019/08/home-2-slide-2-image-1a.jpg" class="d-block w-100" alt="..." />
                <div className="carousel-caption d-none d-md-block">
                  <span className='text-center slide3-content w3-animate-bottom'>
                    <h1 className='display-4 text-dark'>We all love literature</h1>
                    <p className='fs-6 mt-2 ms-5 text-dark'>
                      No.1 best seller in romance,explore more by logging in...<br />
                      and find the amazing deals to buy,explore more about author <br />
                      and the book by logging in
                    </p>
                    <button className='mt-5 text-light fs-3 col-2 rounded-3' style={{ backgroundColor: "#D14031", border: "1px solid #D14031" }}>ReadMore...</button>
                  </span>
                </div>
              </div>
            </div>
            <button className="carousel-control-prev" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="prev">
              <span className="carousel-control-prev-icon" aria-hidden="true"></span>
              <span className="visually-hidden">Previous</span>
            </button>
            <button className="carousel-control-next" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="next">
              <span className="carousel-control-next-icon" aria-hidden="true"></span>
              <span className="visually-hidden">Next</span>
            </button>
          </div>
        </main>
        <section className='row'>
          <div className='col-12'>
            <p className='col-12 col-md-4 col-xl-4 mt-5 display-5 text-center offset-md-4 offset-xl-4 mb-5 arsenal-sc-regular text-light w3-animate-top' style={{ backgroundColor: "rgb(209, 64, 49)" }}>Bestsellers</p>
            <img src="http://localhost:3000/images/hpcs.jpeg" alt="no" className='col-md-2 col-xl-2 col-4 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/pride_prejudice.jpeg" alt="no" className='col-md-2 col-xl-2 col-4 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/great_expectations.jpeg" alt="no" className='col-md-2 col-xl-2 col-4 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/immortals.jpeg" alt="no" className='col-md-2 col-xl-2 col-4 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/hp1.jpeg" alt="no" className='col-md-2 col-4 col-xl-2 me-4 mlh' height="300" />
          </div>
        </section>
        <section className='row'>
          <div className='col-12'>
            <p className='col-12 col-md-4 col-xl-4 mt-5 display-5 text-center offset-md-4 offset-xl-4 mb-5 arsenal-sc-regular text-white  w3-animate-bottom' style={{ backgroundColor: "rgb(209, 64, 49)" }}>Bestselling Authors</p>
            <img src="http://localhost:3000/images/jk.jpeg" alt="no" className='col-md-2 col-xl-2 col-4 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/amish.jpeg" alt="no" className='col-md-2 col-4 col-xl-2 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/ruskin.jpeg" alt="no" className='col-md-2 col-4 col-xl-2 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/rk_narayan.jpeg" alt="no" className='col-md-2 col-4 col-xl-2 me-4 mlh' height="300" />
            <img src="http://localhost:3000/images/jane_austen.jpg" alt="no" className='col-md-2 col-4 col-xl-2 me-4 mlh' height="300" />
          </div>
        </section>
        <footer className='mt-5 row'>
          <Footer></Footer>
        </footer>
      </div>
    </div>

  );
}

export default App;
