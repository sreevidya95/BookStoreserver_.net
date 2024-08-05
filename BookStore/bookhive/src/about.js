import { isMobile } from 'react-device-detect';
export default function Aboutus(){
    return(
     <div className="container-fluid">
        <div className="row">
        <div className="col-12 about">
          <h1 className="display-2 text-center mt-5">About us</h1>
        </div>
        <div className="col-md-4 col-xl-4 col-12 mt-5">
             <img src="/logo.png" alt="no" className={!isMobile && "mt  ms-3"}/>
        </div>
        <div className="col-md-6 col-xl-6 col-12 mt-5">
             <h1 className="display-4 text-center">Who We Are</h1>
             <p className="col-12 fs-5">
             Bookhive is a vibrant online bookstore dedicated to delivering a personalized and immersive literary experience. 
             From the moment visitors land on its welcoming homepage, Bookive strives to engage and inspire book lovers of all ages and interests. 
             The website's intuitive interface seamlessly guides users through a vast collection of titles, categorized by genres, themes, and curated 
             recommendations. Each book listing is accompanied by detailed descriptions, reviews, and multimedia content, including author interviews and
              reader feedback, enriching the browsing experience.
             </p><p className="fs-5">
             Bookive stands out with its innovative features such as a dynamic recommendation engine that suggests books based on individual reading 
             preferences and past purchases. Customers can create personalized bookshelves to organize their virtual libraries and easily share their
              favorite reads with friends and followers. The website also hosts virtual author events and book clubs, connecting readers with their 
              favorite writers through live chats, signings, and interactive discussions.
             </p>
             </div>
             </div>
     </div>
    );
}