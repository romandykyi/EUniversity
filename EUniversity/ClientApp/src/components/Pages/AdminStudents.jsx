import React, {useEffect, useState} from 'react';

const AdminStudents = () => {

    const [students, setStudents] = useState([]);

    const fetchUsers = async() => {
        try {
            const response = await fetch('/api/users/students?Page=1&PageSize=10');
            if (response.ok) {
                const data = await response.json();
                console.log(data);
                setStudents(data.items);
            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }

    }

    useEffect(() => {
        fetchUsers();
    }, []);

    return (
        <div className="students">
            <h1 className="students__title">
                All students
            </h1>
            <table className="table students__table">
                <tr>
                    <th>Email</th>
                    <th>First name</th>
                    <th>Last name</th>
                    <th>Middle Name</th>
                </tr>
                {
                    students.map(student =>
                        <tr key={student.email}>
                            <td>{student.email}</td>
                            <td>{student.firstName}</td>
                            <td>{student.lastName}</td>
                            <td>{student.middleName}</td>
                        </tr>
                    )
                }
            </table>
        </div>
    );
};

export default AdminStudents;