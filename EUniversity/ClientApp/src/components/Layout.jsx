import React from 'react';
import { Container } from 'reactstrap';
import Header from './Header';

const Layout = (props) => {
  return (
    <div>
      <Header />
      <Container tag='main'>{props.children}</Container>
    </div>
  );
};

export default Layout;
