import React, {useState} from 'react';
import Button from "../UI/Button/Button";
import {useNavigate} from "react-router-dom";
import Table from "../UI/Table/Table";

const RegisterNewUsers = () => {

    const [error,setError] = useState('');
    const [isUserAdded, setIsUserAdded] = useState(false);
    const [users, setUsers] = useState([]);
    const navigate = useNavigate();

    const showIsUserAdded = async () => {
        setIsUserAdded(true);
        setTimeout(() => {
            setIsUserAdded(false);
        }, 3000);
    };



    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch("/api/users/students", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(users),
            });

            if (response.ok) {
                console.log("response ok");
                await showIsUserAdded();
            } else {
                console.error("Error:", response.status, response.statusText);
            }
        } catch (error) {
            console.error("An error occurred:", error);
            setError('An error occurred while adding new user.');
        }
    };

    return (
        <div className="newUser form">
            <div className="newUser__title form__title">
                Register new users
            </div>
            <form onSubmit={handleSubmit} className="newUser form__form">
                    <Table users={users} setUsers={setUsers}/>
                    <div className="newUser__error form__error">
                        {error}
                    </div>
                <Button type="submit">Register new users</Button>
                <div className={`newUser__success ${isUserAdded ? "newUser__visible" : "newUser__hidden"}`}></div>
            </form>
        </div>
    );
};

export default RegisterNewUsers;