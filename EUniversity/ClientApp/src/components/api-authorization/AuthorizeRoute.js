import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { ApplicationPaths, QueryParameterNames } from './ApiAuthorizationConstants';
import authService from './AuthorizeService';

export default function AuthorizeRoute({ path, element }) {
  const [ready, setReady] = useState(false);
  const [authenticated, setAuthenticated] = useState(false);

  useEffect(() => {
    const subscription = authService.subscribe(() => authenticationChanged());
    populateAuthenticationState();

    return () => {
      authService.unsubscribe(subscription);
    };
  }, []);

  const populateAuthenticationState = async () => {
    const isAuthenticated = await authService.isAuthenticated();
    setReady(true);
    setAuthenticated(isAuthenticated);
  };

  const authenticationChanged = async () => {
    setReady(false);
    setAuthenticated(false);
    await populateAuthenticationState();
  };

  if (!ready) {
    return <div></div>;
  }

  const link = document.createElement("a");
  link.href = path;

  return authenticated ? element : <Navigate replace to="/login" />;
}
