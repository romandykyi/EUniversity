import React, {useState} from 'react';
import Button from "../UI/Button/Button";
import Table from "../UI/Table/Table";

const RegisterNewUsers = () => {

    const [error,setError] = useState('');
    const [isUserAdded, setIsUserAdded] = useState(false);
    const [users, setUsers] = useState([]);
    const [postUsers, setPostUsers] = useState([]);

    const showIsUserAdded = async () => {
        setIsUserAdded(true);
        setTimeout(() => {
            setIsUserAdded(false);
        }, 3000);
    };



    const handleSubmit = async (e) => {
        e.preventDefault();

        const postUsers = users.map(user => (
            {
                email: user.email,
                firstName: user.firstName,
                lastName: user.lastName,
                middleName: user.middleName,
            }
            ));

        try {
            const response = await fetch("/api/users/students", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.ok) {
                await showIsUserAdded();
            } else {
                console.error("Error:", response.status, response.statusText);
                setError(`${response.status} ${response.statusText}`);
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
                    <Table
                        users={users}
                        setUsers={setUsers}
                        postUsers = {postUsers}
                        setPostUsers = {setPostUsers}
                    />
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