import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter,Routes,Route } from 'react-router-dom';
import UserLogin from './userlogin';
import AdminLogin from './adminlogin';
import Books from './Books';
import BookDetails from './BookDetails';
import Authors from './authors';
import AuthorDetails from './AuthorDetails';
import FileNotFound from './FileNotFound';
import Aboutus from './about';
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <BrowserRouter>
    <Routes>
        <Route path='/' element={<App />}/>
        <Route path='/userLogin' element={<UserLogin/>}/>
        <Route path="/adminLogin" element={<AdminLogin/>}/>
        <Route path="/adminLogin/:cp" element={<AdminLogin/>}/>
        <Route path="/books" element={<Books/>}/>
        <Route path="/book/:id" element={<BookDetails/>}/>
        <Route path='/authors' element={<Authors/>}/>
        <Route path='/author/:id' element={<AuthorDetails/>}/>
        <Route path="/about" element={<Aboutus/>}/>
        <Route path="*" element={<FileNotFound/>}/>
    </Routes>
    </BrowserRouter>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
