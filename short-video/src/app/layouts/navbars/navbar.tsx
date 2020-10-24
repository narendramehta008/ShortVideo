import React, { Component, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Navbar, Nav, NavDropdown } from "react-bootstrap";
import { Button, Card, IconButton } from "ui-neumorphism";
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


  return (
    <Navbar collapseOnSelect expand="lg" style={{ background: '#b9cae2' }} variant="dark">
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

          <NavDropdown title="Download Pages" id="basic-nav-dropdown">
            <NavDropdown.Item as={Link} to="/download-image">
              Download Image
            </NavDropdown.Item>
          </NavDropdown>

          {user ? <NavDropdown title="" id="basic-nav-dropdown" className="user-avatar">
            <NavDropdown.Item as={Link} to="/download-image"  >
              <Card style={{ margin: '-10px -20px', display: 'list-item', alignItems: 'center' }}>
                <Icon path={mdiAccount} size={2} color="var(--primary)" /> <br></br>
                <Label>{user}</Label>
              </Card>
            </NavDropdown.Item>
          </NavDropdown> : null}

        </Nav>
      </Navbar.Collapse>
      {/* <Form inline>
    <FormControl type="text" placeholder="Search" className="mr-sm-2" />
    <Button variant="outline-light">Search</Button>
  </Form> */}
    </Navbar >
  );
}
