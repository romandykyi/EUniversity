import React from 'react';

const TableOfStudents = () => {

    // TODO: fetch students from /api/users/students

    const students = [{ email: "sample@example.com", firstName: "Student", lastName: "Student" }];

    // TODO: implement pagination

    return (<table className="table students__table">
        <tr>
            <th>Email</th>
            <th>First name</th>
            <th>Last name</th>
            <th>Middle Name</th>
        </tr>
        {students.map((item) => {
            return (
                <tr key={item.email}>
                    <td>{item.email}</td>
                    <td>{item.firstName}</td>
                    <td>{item.lastName}</td>
                    <td>{item.middleName}</td>
                </tr>
            );
        })}
    </table>);
};

export default TableOfStudents;