import React, { useEffect, useState } from 'react';
import authService from '../api-authorization/AuthorizeService';

const Home = () => {

    const [role, setRole] = useState('');

    useEffect(() => {
        const getRole = async() => {
            const user = await authService.getUser();
            if (user !== null) setRole(user.role);
            else setRole('user');
        };

        getRole();
    }, [])

    return (
        <div className="w-full h-full flex justify-start items-center pt-20 container max-w-[1100px]">
            <h1 className="text-3xl">Hello {role}!</h1>

        </div>
    );
};

Home.displayName = Home.name;

export default Home;