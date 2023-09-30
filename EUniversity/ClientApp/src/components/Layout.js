import React from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';
import PasswordChangedBlock from "./UI/PasswordChangedBlock/PasswordChangedBlock";

const Layout = (props) => {
    return (
        <div>
            <NavMenu />
            <Container tag="main">
                {props.children}
            </Container>

        </div>
    );
}

export default Layout;
