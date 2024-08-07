import { Link, useNavigate } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import Modal from "react-bootstrap/Modal"
import Button from "react-bootstrap/Button"
import { useParams } from "react-router-dom";
import { postData, postM } from "./fetch";
import Model from "./modal";
export default function AdminLogin() {
  const navigate = useNavigate();
  const [passwordType, setPasswordType] = useState(true);
  const [pwd, setpassword] = useState({ newpassword: "", confirm: "" })
  const [toast, setToast] = useState(false);
  const [modal, setModal] = useState(false);
  const [fp, setfp] = useState(false);
  const [err, setErr] = useState({});
  const [loading, setLoading] = useState(false);
  const [types, setType] = useState("password");
  const cp = useParams();
  let val = useRef();
  let error = {};
  const [data, setData] = useState({ email: "", password: "" });
  useEffect(() => {
    if (cp.hasOwnProperty('cp')) {
      setToast(true)
      setfp(true);
    }
  }, [])
  const [forgotEmail, setForgotEmail] = useState();
  function forgotPassword() {
    setToast(true);
    setfp(false);
  }
  async function Login(event) {
    event.preventDefault();
    let error = {};
    if (data.email === '') {
      error.email = "Invalid Email";
    }
    else if (data.password === '') {
      error.password = "Invalid Password";
    }
    else {
      setLoading(true);
      error = {};
      let message = await postData("https://localhost:7136/Login", "Post", data);
      console.log(message);
      if (message.hasOwnProperty('admin_id')) {
        setLoading(false);
        setToast(false);
        setModal(true);
        localStorage.setItem('id', message.admin_id);
        localStorage.setItem('name', message.name);
        localStorage.setItem('email', message.email);
        localStorage.setItem("authenication",message.authenticationToken)
        navigate("/books");
        
      }
      else {
        setLoading(false);
        val.current =message;
        setToast(false);
        setModal(true)

      }
    }
    setErr(error);
  }
  function toggleType() {
    if (types === 'password') {
      setType("text");
    }
    else {
      setType("password");
    }
  }
  async function sendMail() {
    if (forgotEmail === '' || typeof forgotEmail === 'undefined') {
      error.email = "Email should not be empty";
    }
    else if (/\S+@\S+\.\S+/.test(forgotEmail) === false) {
      error.email = "Enter valid Email";
    }
    else {
      setLoading(true);
      const message = await postM("https://localhost:7136/Admin/fp", "Post", { email: forgotEmail });
      if (message==200) {
        setLoading(false);
        val.current = "message sent";
        setToast(false);
        setModal(true)

      }
      else{
        setLoading(false);
        val.current = "Greetings Never Sent";
        setToast(false);
        setModal(true)
      }
    }
    setErr(error);
  }
  async function updatePassword() {
    let error = {}
    if (pwd.newpassword === '') {
      error.newpassword = "new password field  should not be empty";
    }
    else if (!(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@.#$!%*?&^])[A-Za-z\d@.#$!%*?&]{8,15}$/.test(pwd.newpassword))) {
      error.newpassword = "password should contain uppercase,lowercase numeric and symbols";
    }
    else if (pwd.newpassword !== pwd.confirm) {
      error.confirm = "New Password and Confirm new password should be same";
    }
    else {
      error = {};
      const message = await postM(`https://localhost:7136/Admin/newPassword/${cp.cp}`, "put", { password: pwd.newpassword });
      if (message===200) {
        
        setLoading(false);
        setToast(false);
        setModal(false);
        navigate("/adminLogin");
        
      }
      else {
        val.current = "Something went wrong";
        setToast(false);
        setModal(true)
      }
    }
    setErr(error);
  }
  function handleClose() {
    setModal(false);
  }
  function passwordChange(event) {
    setpassword({ ...pwd, [event.target.name]: event.target.value });
  }
  function handleLoginChange(event) {
    setData({ ...data, [event.target.name]: event.target.value })
  }
  return (
    <div className="container">
      {loading ?
        <div className="row">
          <div className="spinner-grow sp col-6 offset-6" role="status">
            <span className="sr-only">Loading...</span>
          </div>
        </div>
        :
        <>
          <div className="row mt-5">
            <img className="col-md-4 col-xl-4 col-12 img" src="/logo.png" alt="no" height="600" />
            <div className="col-md-5 col-xl-5 col-12 border border-secondary mt">
              <h3 className="h1 text-center col-12 margin arsenal-sc-regular">Admin Login</h3>
              <form className="col-12" onSubmit={Login}>
                <input type="email" className="col-5 offset-3 form-control-sm mt-5" placeholder="Enter  Email Id " id="email" name="email" onChange={handleLoginChange} required />
                {err.email && <h6 className="h6 col-8 text-danger">{err.email}</h6>}
                <div className="input-group  mt-4">
                  <input type={passwordType ? "password" : "text"} className="form-control-sm col-5 offset-3" placeholder="Enter Password" onChange={handleLoginChange} name="password" id="password" />
                  {passwordType ? <span><i class="fa fa-eye-slash col-1" aria-hidden="true" onClick={() => setPasswordType(false)}></i></span> : <span><i class="fa fa-eye col-1" aria-hidden="true" onClick={() => setPasswordType(true)}></i></span>}
                </div>
                {err.password && <h6 className="h6 col-5 offset-3 text-danger">{err.password}</h6>}
                <Link className="btn col-6 offset-4 mt-3 text-primary text-decoration-underline" onClick={forgotPassword}>Forgot password?</Link>
                <input type="submit" className="btn-color col-3 offset-4 mt-5 text-white rounded-2 mb-2" value="Login" />
              </form>
            </div>
          </div>
          {modal && <Model show={modal} msg={val.current} onClick={handleClose} />}
          <Modal show={toast}>
            <Modal.Header>
              <Modal.Title>
                <h5>{fp ? "Update Password" : "Forgot Password"}</h5>
              </Modal.Title>
            </Modal.Header>
            <Modal.Body>
              {fp ? (
                <>
                  <input type={types} placeholder="New Password" className={`col-8 form-control-sm ${err.newpassword ? "border border-danger" : "border border-secondary"}`} name="newpassword" required onChange={passwordChange} /><br />
                  {err.newpassword && <h6 className="h6 col-8 text-danger">{err.newpassword}</h6>}
                  <input type={types} placeholder="Confirm New Password" className={`col-8 form-control-sm  mt-2 ${err.confirm ? "border border-danger" : " border border-secondary"}`} name="confirm" onChange={passwordChange} required />
                  {err.confirm && <h6 className="h6 col-8 text-danger">{err.confirm}</h6>}
                  <div className="form-check col-12 mt-2">
                    <input className="form-check-input border border-secondary" type="checkbox" value="" id="flexCheckDefault" onClick={toggleType} />
                    <label className="form-check-label ms-1" for="flexCheckDefault">
                      Show Password
                    </label>
                  </div>
                </>
              ) :
                <>
                  <input type="email" placeholder="Enter your Email" className='col-8' name="forgot_email" onChange={(e) => setForgotEmail(e.target.value)} required />
                  {err.email && <h6 className="h6 col-8 text-danger">{err.email}</h6>}
                </>
              }
            </Modal.Body>
            <Modal.Footer>
              {fp ? (
                <Button variant="primary" onClick={updatePassword}>
                  Submit
                </Button>
              ) :
                <Button variant="primary" onClick={sendMail}>
                  Send Email
                </Button>
              }
              <Button variant="secondary" onClick={() => { setToast(false); setErr({}); }}>
                Close
              </Button>
            </Modal.Footer>
          </Modal>
        </>}
    </div>
  )
}