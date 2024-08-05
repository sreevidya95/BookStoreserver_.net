import { isMobile } from "react-device-detect";
export default function Footer() {
    return (
        <div className="row footer">
            <div className="col-12 col-md-3 col-xl-3 ${!isMobile && 'ms-5">
                <h3 className="col-12 h4 arsenal-sc-regular fw-bold text-light text-center">Publishers</h3>
                <h6 className="fs-6 col-12  text-light text-center">Bestsellers</h6>
                <h6 className="fs-6 col-12  text-light text-center">Author Story</h6>
                <h6 className="fs-6 col-12  text-light text-center">Book Fairs</h6>
                <h6 className="fs-6 col-12  text-light text-center">Help(FAQ)</h6>
            </div>
            <div className="col-12 col-md-3 col-xl-3">
                <h3 className="col-12 h4 arsenal-sc-regular fw-bold text-light text-center">Contact</h3>
                <h6 className="fs-6 col-12 text-light text-center">Bhive Bookstore</h6>
                <h6 className="fs-6 col-12 text-light text-center">Floor 12, Prestige Techpark</h6>
                <h6 className="fs-6 col-12  text-light text-center">Bangalore</h6>
                <h6 className="fs-6 col-12  text-light text-center">India</h6>
            </div>
            <div className="col-12 col-md-3 col-xl-3">
                <h3 className="col-12 h4 arsenal-sc-regular fw-bold text-light text-center">News And Update</h3>
                <h6 className="fs-6 col-12 text-light text-center">We’d love it if you subscribed to our newsletter!<br /> You’ll love it too.</h6>
                <h6 className="fs-6 col-12  text-light text-center"><input type="email" placeholder="Email us" /></h6>
            </div>
            <div className="col-12 col-md-2 col-xl-2">
                <h3 className="col-12 h4 arsenal-sc-regular fw-bold text-light text-center">Social Media</h3>
                <h6 className="fs-6 col-12 text-light text-center">
                    <span className='col-1 me-3'><i className="fa-brands fa-x-twitter text-light"></i></span>
                    <span className='col-1 me-3'><i className="fa-brands fa-square-instagram text-light"></i></span>
                    <span className='col-1 me-3'><i className="fa-brands fa-facebook text-light"></i></span>
                </h6>
            </div>
            <div className="col-12" style={{ backgroundColor: "#000" }}>
                <h6 className="h6 col-12  text-light text-center">&copy; 2024, All Rights reserved</h6>
            </div>
        </div>

    );
}