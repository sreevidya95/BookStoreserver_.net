import { useState } from "react";
import { postData } from './fetch'
import Model from "./modal";
export default function UserLogin() {
    const [loading, setLoading] = useState(false);
    const [enquiry, setEnquiry] = useState({ user_email: "", message: "" });
    const [toast, setToast] = useState(false);
    function handleChange(event) {
        setEnquiry({ ...enquiry, [event.target.name]: event.target.value });
    }
    const submitEnquiry = async (event) => {
        event.preventDefault();
        setLoading(true);
        const message = await postData("http://localhost:3000/enquiry/", "POST", enquiry);
        if (message.hasOwnProperty('message')) {
            setToast(true);
            setLoading(false);
        }
    }
    function handleClose() {
        setToast(false);
        setEnquiry({ user_email: "", message: "" })
    }
    return (
        <div className="container-fluid bg-img">
            {loading ?
                <div className="spinner-grow" role="status">
                    <span className="sr-only">Loading...</span>
                </div>
                :
                <>
                    <div className="row">
                        <h5 className="h4 text-center mt-5 col-6 offset-3">Sorry,Our User Login page is currently under maintanance,please write your enquiries,Feedback or send your
                            address in below form, we will reply to you through Email.
                        </h5>
                    </div>
                    <form className="row form mt-5 auth" onSubmit={submitEnquiry}>
                        <input type="email" className="form-control-sm col-6 col-md-3 col-xl-3 offset-md-4 offset-3 mb-3 border border-secondary" placeholder="Enter your email id" name="user_email" onChange={handleChange} required />
                        <textarea className="form-control-sm col-6 col-md-3 ocol-xl-3 offset-md-4 mb-3 offset-3" placeholder="Enter your Enquiries or your address and book name here" onChange={handleChange} name="message" rows="5" required></textarea>
                        <input type="submit" className="col-md-2 col-4 col-xl-1 offset-md-5 btn btn-info mb-5 offset-4" />
                    </form>
                    {toast && <Model show={toast} onClick={handleClose} msg="Enquiry Sent" />}
                </>
            }
        </div>
    );
}