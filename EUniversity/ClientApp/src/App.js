    import React from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import Layout from "./components/Layout";
import './custom.css';
import './styles/style.css';
import Login from "./components/Pages/Login";
//To do
    // end a PasswordChangeBlock when password changes successfully
    // add Redux to a project to realize some features with global states
const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path="/login" element={<Login/>} />
                {AppRoutes.map((route, index) => {
                    const { element, requireAuth, ...rest } = route;
                    return (
                        <Route
                            key={index}
                            {...rest}
                            element={requireAuth ? <AuthorizeRoute {...rest} element={element} /> : element}
                        />
                    );
                })}
            </Routes>
        </Layout>
    );
};
App.displayName = 'App';

export default App;