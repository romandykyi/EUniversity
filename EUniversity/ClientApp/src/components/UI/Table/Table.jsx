import React, { useState } from 'react';
import styles from "./Table.module.css";
import Button from "../Button/Button";

const Table = ({users, setUsers, postUsers, setPostUsers}) => {

    const handleInputChange = (id, field, value) => {
        const newData = users.map((row) =>
            row.id === id ? { ...row, [field]: value } : row
        );
        setUsers(newData);
    };


    const handleAddUserClick = e => {
        e.preventDefault();
        const newUser = {
            id:Date.now(),
            email:'',
            firstName:'',
            lastName:'',
            middleName:''
        };

        setUsers([...users, newUser]);
    }
    const deleteUser = (id) => {

        const newData = users.filter(user => user.id !== id);
        setUsers(newData);
    }

    return (
        <div className={`${styles.tableDiv}`}>
            <table className={`table ${styles.table}`}>
                <thead>
                <tr>
                    <th>Email</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Middle Name</th>
                    <th>Action</th>
                </tr>
                </thead>
                <tbody>
                {users.map((row) => (
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
                           <Button onClick = {e => {
                               e.preventDefault();
                               deleteUser(row.id);
                           }}>Delete</Button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
            <Button onClick={handleAddUserClick}>Add new user</Button>
        </div>
    );
};

export default Table;
