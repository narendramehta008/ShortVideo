import React, { Suspense, useEffect } from "react";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { NavBar } from "./app/layouts/navbars/navbar";
import About from "./app/pages/about";
import Home from "./app/pages/home";

//import "./styles/main.scss";
import { DownloadMain } from "./features/download-images/downloadmain";
import "semantic-ui-css/semantic.min.css";
import { Login } from "./app/pages/auth/login";
import 'ui-neumorphism/dist/index.css'
import { NavLink } from "react-bootstrap";
import { CustomLogin } from "./app/pages/auth/custom-login";
//import { overrideThemeVariables } from 'ui-neumorphism'


function App() {

  useEffect(() => {
    // overrideThemeVariables({
    //   '--light-bg': '#E9B7B9',
    //   '--light-bg-dark-shadow': '#ba9294',
    //   '--light-bg-light-shadow': '#ffdcde',
    //   '--dark-bg': '#292E35',
    //   '--dark-bg-dark-shadow': '#21252a',
    //   '--dark-bg-light-shadow': '#313740',
    //   '--primary': '#8672FB',
    //   '--primary-dark': '#4526f9',
    //   '--primary-light': '#c7befd'
    // })
  }, [])
  const token = localStorage.getItem('token');

  return (
    <div>
      <Router>
        <NavBar />
        <Suspense fallback={<div>loading...</div>}>
          <Switch>
            {/* <NavLink></NavLink> */}
            <Route path="/about">
              <About />
            </Route>

            <Route path="/download-image">
              <DownloadMain />
            </Route>

            <Route path="/">
              {token ? < Home /> : <CustomLogin />}
            </Route>
          </Switch>
        </Suspense>
      </Router>
    </div>
  );
}

export default App;
