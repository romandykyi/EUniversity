import React, {useEffect, useState} from 'react';
import TableOfStudents from "../TableOfStudents";

const AdminStudents = () => {

    const [students, setStudents] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(0);

    const fetchUsers = async() => {
        try {
            const response = await fetch('/api/users/students?Page=1&PageSize=10');
            if (response.ok) {
                const data = await response.json();
                setStudents(data.items);
                setItemsPerPage(data.pageSize);
                setIsLoading(false);
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
            <h1 className="students__title form__title">
                All students
            </h1>
            <TableOfStudents itemsPerPage={itemsPerPage} items={students} isLoading={isLoading}/>
        </div>

    );
};

export default AdminStudents;