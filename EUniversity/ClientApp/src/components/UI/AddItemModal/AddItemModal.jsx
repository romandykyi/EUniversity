import { useState } from "react";
import Table from "../Table/Table";
import Button from "../Button/Button";

const AddItemModal = ({
    isVisible,
    setIsVisible,
    fetchItems
}) => {

    const [error,setError] = useState('');
    const [users, setUsers] = useState([]); 

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        const students = [...users.filter(user => user.role === "student"), ...users.filter(user => !user.role)];
        const teachers = users.filter(user => user.role === "teacher");

        const postStudents = students.map(user => (
            {
                email: user.email,
                firstName: user.firstName,
                lastName: user.lastName,
                middleName: user.middleName,
            }
            ));
        const postTeachers = teachers.map(user => (
            {
                email: user.email,
                firstName: user.firstName,
                lastName: user.lastName,
                middleName: user.middleName,
            }
        ));

        if (students.length) await getResponse("students", postStudents);
        if (teachers.length) await getResponse("teachers", postTeachers);
        setUsers([]);
        await fetchItems();
    };

    const getResponse = async(usersType, usersList) => {
        try {
            const response = await fetch(`/api/users/${usersType}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: `{"users": ${JSON.stringify(usersList)}}`,
            });

            if (response.ok) {
                console.log('ok');
                setIsVisible(false);
                document.body.style.overflow = 'auto';
            } else {
                console.error("Error:", response.status, response.statusText);
                setError(`${response.status} ${response.statusText}`);
            }
        } catch (error) {
            console.error("An error occurred:", error);
            setError('An error occurred while adding new user.');
        }
    };

    const handleClickOnBg = () => {
        setIsVisible(false);
        document.body.style.overflow = 'auto';
    };

    const handleInputChange = (id, field, value) => {
        const newData = users.map((row) =>
            row.id === id ? { ...row, [field]: value } : row
        );
        setUsers(newData);
    };

    const deleteUser = (id) => {
        const newData = users.filter(user => user.id !== id);
        setUsers(newData);
    };

    return (
        <div 
            onClick={handleClickOnBg}
            className={`${isVisible ? "absolute" : "hidden"} top-0 bottom-0 left-0 right-0 bg-black bg-opacity-50 z-30 flex items-center justify-center px-4`}
        >
            <div 
                className=" container max-w-[1100px] pt-10 bg-white p-10 rounded-lg" 
                onClick={(e) => e.stopPropagation()}
            >
                <h1 className="newUser__title form__title">
                    Register new users
                </h1>
                <form onSubmit={handleSubmit} className="newUser form__form">
                        <Table
                            items={users}
                            setItems={setUsers}
                            title="users"
                            itemParams={{
                                email:'',
                                firstName:'',
                                lastName:'',
                                middleName:''
                            }}
                            tableHead={(
                                <tr>
                                    <th>Email</th>
                                    <th>First Name</th>
                                    <th>Last Name</th>
                                    <th>Middle Name</th>
                                    <th>Role</th>
                                    <th>Delete</th>
                                </tr>
                            )}
                            tableBody={users.map((row) => (
                                <tr key={row.id}>
                                    <td>
                                            <input
                                                type="text"
                                                placeholder="email"
                                                value={row.email}
                                                onChange={(e) => handleInputChange(row.id, 'email', e.target.value)}
                                            />
            
                                    </td>
                                    <td>
                                            <input
                                                type="text"
                                                placeholder="first name"
                                                value={row.firstName}
                                                onChange={(e) => handleInputChange(row.id, 'firstName', e.target.value)}
                                            />
            
                                    </td>
                                    <td>
                                            <input
                                                type="text"
                                                value={row.lastName}
                                                placeholder="last name"
                                                onChange={(e) => handleInputChange(row.id, 'lastName', e.target.value)}
                                            />
            
                                    </td>
                                    <td>
                                            <input
                                                type="text"
                                                placeholder="middle name"
                                                value={row.middleName}
                                                onChange={(e) => handleInputChange(row.id, 'middleName', e.target.value)}
                                            />
            
                                    </td>
                                    <td> 
                                        <select 
                                            className="form-select w-40" 
                                            onChange={(e) => handleInputChange(row.id, 'role', e.target.value)}
                                        >
                                            <option value="student">student</option>
                                            <option value="teacher">teacher</option>
                                        </select>
                                    </td>
                                    <td>
                                       <Button onClick = {e => {
                                           e.preventDefault();
                                           deleteUser(row.id);
                                       }}>Delete</Button>
                                    </td>
                                </tr>
                            ))}
                        />
                        <div className="newUser__error form__error">
                            {error}
                        </div>
                    <Button type="submit">Register new users</Button>
                </form>
            </div>
        </div>
    );
};

export default AddItemModal;