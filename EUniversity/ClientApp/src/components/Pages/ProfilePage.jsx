import { useEffect, useState } from 'react';
import authService from '../api-authorization/AuthorizeService';

const ProfilePage = () => {

    const [user, setUser] = useState({});
    
    useEffect(() => {
        const getUserInfo = async() => {
            const user = await authService.getUser();
            console.log(user);
            setUser(user);
        }
        getUserInfo();
    }, [])


    return (
        <div className="students container max-w-[1100px] pt-10">
            <h1 className="students__title form__title">
            {user.family_name} {user.given_name}
            </h1>
            <ul className="flex flex-col gap-2">
                <li className="text-text text-xl">Email: {user.email}</li>
                <li className="text-text text-xl">Role: {user.role}</li>
            </ul>
        </div>
    );
};

export default ProfilePage;