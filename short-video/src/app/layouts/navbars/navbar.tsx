import React, { Component, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Navbar, Nav, NavDropdown } from "react-bootstrap";
import { Button, Card, IconButton, } from "ui-neumorphism";
import { mdiInformation, mdiHome, mdiAccount } from '@mdi/js';
import Icon from "@mdi/react";
import './navbar.scss'
import { Label } from "semantic-ui-react";

interface IProps {
  dark?: boolean,
}
export const NavBar: React.FC<IProps> = ({ dark }) => {

  const [user, setUser] = useState("")
  useEffect(() => {
    let user = localStorage.getItem('username');
    user && setUser(user);
  }, [])

  const logout = () => {
    localStorage.removeItem('username');
    localStorage.removeItem('token');
    window.location.reload();
  }

  // const CustomToggle = React.forwardRef(({ children, onClick: any }, ref: any) => (
  //   <a
  //     href=""
  //     ref={ref}
  //     onClick={e => {
  //       e.preventDefault();
  //       onClick(e);
  //     }}
  //   >
  //     {/* Render custom icon here */}
  //     &#x25bc;
  //     {children}
  //   </a>
  // ));

  return (
    <Navbar collapseOnSelect expand="lg" style={{ background: '#ecf0f3' }} variant="dark">
      {/* <Navbar.Brand href="/" className="fab fa-twitter"></Navbar.Brand> */}
      <Navbar.Toggle aria-controls="responsive-navbar-nav" />
      <Navbar.Collapse id="responsive-navbar-nav">
        <Nav className="mr-auto">
          <Link to="/" className="nav-link">
            <IconButton
              size="medium"
              rounded
              text={false}
              color="var(--primary)"
              dark={dark}  >
              <Icon path={mdiHome} size={1} />
            </IconButton>
          </Link>
          <Link to="/about" className="nav-link">
            {/* font-family: 'Lato', sans-serif; */}
            <Card style={{ padding: '5px', fontFamily: 'Lato ,sans-serif' }}>
              <Icon path={mdiInformation} size={1} color="var(--primary)" title="About" />
              <label>About</label>
            </Card>

          </Link>


          {user ?
            <div style={{ display: 'flex' }} className="nav-item dropdown">
              {/* <NavDropdown title="" className="fas fa-cloud" style={{ width: '50px', height: '50px' }} id="basic-nav-dropdown" > */}
              <NavDropdown title="" id="basic-nav-dropdown" className="user-avatar">
                <NavDropdown.Item as={Link} to="/download-image" style={{ background: '#ECF0F3' }}>
                  <Button style={{ width: '140%', padding: '0', marginLeft: '-18px', border: 'none' }}  >account</Button>
                </NavDropdown.Item>
              </NavDropdown>


              <NavDropdown title="" id="basic-nav-dropdown" className="user-avatar">
                <NavDropdown.Item as={Link} to="/download-image" style={{ background: '#ECF0F3' }}>
                  <Card style={{ width: '140%', padding: '0', marginLeft: '-20px' }}  >
                    <Icon path={mdiAccount} size={2} color="var(--primary)" /> <br />
                    <Label>{user}</Label>
                  </Card>
                </NavDropdown.Item>

                <NavDropdown.Item as={Link} to="/download-image" style={{ background: '#ECF0F3' }}>
                  <Button style={{ width: '140%', padding: '0', marginLeft: '-18px', border: 'none' }}  >account</Button>
                </NavDropdown.Item>

                <NavDropdown.Item as={Link} to="/" style={{ background: '#ECF0F3' }}>
                  <Button style={{ width: '140%', padding: '0', marginLeft: '-20px', border: 'none' }} onClick={(ev) => { logout() }} >Log Out</Button>
                </NavDropdown.Item>

              </NavDropdown>
            </div>
            : null}

        </Nav>
      </Navbar.Collapse>
      {/* <Form inline>
    <FormControl type="text" placeholder="Search" className="mr-sm-2" />
    <Button variant="outline-light">Search</Button>
  </Form> */}
    </Navbar >
  );
}
