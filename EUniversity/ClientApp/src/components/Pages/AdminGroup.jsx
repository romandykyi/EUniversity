import {React, useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from '../../store/store';
import Button from "../UI/Button/Button";
import DeleteModal from '../UI/DeleteModal/DeleteModal';

const AdminGroup = () => {


    const [students, setStudents] = useState([]);
    const [teacher, setTeacher] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [deletedUser, setDeletedUser] = useState({
        id: '',
        name: ''
    });
    const location = useLocation();
    const regex = /\/groups\/(\d+)/;
    const groupNumber = location.pathname.match(regex)[1];
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    useEffect(() => {
        fetchGroup();
    }, []);

    const fetchGroup = async(page = 1, pageSize = 10) => {
        try {
            const response = await fetch(`/api/groups/${groupNumber}`);
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
    
    const deleteUserFromGroup = async (userId) => {
        try {
            const response = await fetch(`/api/groups/${groupNumber}/students/${userId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                console.log(`deleted user: ${userId}`);
                fetchGroup();
                setIsDeleteVisible();
            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };

    return (
        <>
            <DeleteModal
                setIsVisible={setIsDeleteVisible}
                isVisible={isDeleteVisible}
                itemType="student"
                deleteFunction={deleteUserFromGroup}
                deletedUser={deletedUser}

            />
            <div className="students container max-w-[1100px] pt-10">
                <h1 className="students__title form__title">
                    Group #{groupNumber}
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
                                <th>Username</th>
                                {
                                    isAdmin 
                                        ? <th>Delete</th>
                                        : ""
                                }
                            </tr>
                    </thead>
                    <tbody>
                    {
                            students.map((item) => (
                                <tr key={item.id}>
                                    <td>{item.firstName}</td>
                                    <td>{item.lastName}</td>
                                    <td>{item.userName}</td>
                                    {
                                    isAdmin 
                                        ? <th><Button onClick={() => {
                                            setIsDeleteVisible(true);
                                            setDeletedUser({id: item.id, name: `${item.firstName} ${item.lastName}`});
                                        }}>Delete user</Button></th>
                                        : ""
                                    }
                                </tr>
                            ))
                        }
                    </tbody>
                    </table>
                </div>
            </div>
        </>
    );
};

export default AdminGroup;