import React, { Suspense } from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import Navbar from "./app/layouts/navbar";
import About from "./app/pages/about";
import Home from "./app/pages/home";
import { AccordionProps } from "react-bootstrap";

//import "./styles/main.scss";
import { DownloadMain } from "./features/download-images/downloadmain";
import "semantic-ui-css/semantic.min.css";
import { Login } from "./app/pages/auth/login";




function App() {

  const token = localStorage.getItem('token');

  return (
    <div>
      <Router>
        <Navbar />
        <Suspense fallback={<div>loading...</div>}>
          <Switch>
            <Route path="/about">
              <About />
            </Route>

            <Route path="/download-image">
              <DownloadMain />
            </Route>

            <Route path="/">
              {token ? < Home /> : <Login />}
            </Route>
          </Switch>
        </Suspense>
      </Router>
    </div>
  );
}

export default App;
