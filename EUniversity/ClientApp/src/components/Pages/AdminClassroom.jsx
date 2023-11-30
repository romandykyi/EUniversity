import {React, useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';

const AdminClassroom = () => {

    const [name, setName] = useState('');
    const [students, setStudents] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const location = useLocation();
    const regex = /\/classrooms\/(\d+)/;
    const classroomNumber = location.pathname.match(regex)[1];

    const fetchClassroom = async(page = 1, pageSize = 10) => {
        try {
            const response = await fetch(`/api/classrooms/${classroomNumber}`);
            if (response.ok) {
                const data = await response.json();
                setName(data.name);
                setIsLoading(false);

            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }

    };

    useEffect(() => {
        fetchClassroom();
    }, []);

    return (
        <div className="students container max-w-[1100px] pt-10">
            <h1 className="students__title form__title">
                Classroom #{classroomNumber} - {name}
            </h1>
            {
                students.length
                ?   <>
                        <div className="table-container">
                            <table className="table students__table">
                            <thead>
                                <tr>
                                        <th>First name</th>
                                        <th>Last name</th>
                                        <th>username</th>
                                    </tr>
                            </thead>
                            <tbody>
                            {
                                    students.map((item) => (
                                        <tr key={item.id}>
                                            <td>{item.firstName}</td>
                                            <td>{item.lastName}</td>
                                            <td>{item.userName}</td>
                                        </tr>
                                    ))
                                }
                            </tbody>
                            </table>
                        </div>
                    </>
                : <p className="text-gray-400 text-5xl text-center mt-[200px] fw-bold">No students in this classroom</p>
            }
        </div>

    );
};

export default AdminClassroom;