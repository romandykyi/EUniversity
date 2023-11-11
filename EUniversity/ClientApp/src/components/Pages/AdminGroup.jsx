import {React, useEffect, useState} from 'react';
import { useAppSelector } from '../../store/store';
import { useLocation } from 'react-router-dom';

const AdminGroup = () => {

    const group = useAppSelector(state => state.currentGroup.currentGroup);

    const [students, setStudents] = useState([]);
    const [teacher, setTeacher] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const location = useLocation();
    const regex = /\/groups\/(\d+)/;
    const groupNumber = location.pathname.match(regex)[1];

    const fetchGroup = async(page = 1, pageSize = 10) => {
        try {
            const response = await fetch(`/api/groups/${!group.id ? groupNumber : group.id}`);
            if (response.ok) {
                const data = await response.json();
                setStudents(data.students);
                setTeacher(data.teacher);
                setIsLoading(false);

            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }

    };

    useEffect(() => {
        fetchGroup();
    }, []);

    return (
        <div className="students container max-w-[1100px] pt-10">
            <h1 className="students__title form__title">
                Group #{!group.id ? groupNumber : group.id}
            </h1>
            <h2 className="text-3xl font-bold mb-5">
                Teacher: {teacher.firstName} {teacher.lastName}
            </h2>
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
        </div>

    );
};

export default AdminGroup;