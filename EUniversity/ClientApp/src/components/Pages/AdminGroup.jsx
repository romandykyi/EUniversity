import {React, useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from '../../store/store';
import Button from "../UI/Button";
import AddItemToGroupModal from '../AddItemToGroupModal';
import DeleteModal from '../DeleteModal';
import BackButton from '../UI/BackButton';

const AdminGroup = () => {


    const [students, setStudents] = useState([]);
    const [teacher, setTeacher] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [isAddStudentVisible, setIsAddStudentVisible] = useState(false);
    const isThemeDark = useAppSelector(state => state.theme.isThemeDark);
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
                setIsDeleteVisible(false);
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
                deletedItem={deletedUser}
            />
            <AddItemToGroupModal
                isVisible={isAddStudentVisible}
                setIsVisible={setIsAddStudentVisible}
                title="student"
                fetchTitle="groups"
                groupId={groupNumber}
                fetchItems={fetchGroup}
            />
            <div className="students container max-w-[1100px] pt-10">
                <div className="flex items-center gap-3 mb-14">
                    <BackButton navigate="groups"/>
                    <h1 className="students__title form__title mb-0">
                    Group #{groupNumber}
                    </h1>
                </div>
                <h2 className="text-3xl font-bold mb-5">
                    Teacher: {teacher.firstName} {teacher.lastName}
                </h2>
                {
                    isAdmin 
                    ?   <Button onClick={() => setIsAddStudentVisible(true)}>Add student to group</Button>
                    :   ""
                }
                {
                    students.length
                    ?   <>
                            <div className="table-container mt-5">
                                <table className={`table table-hover ${isThemeDark ? 'table-dark' : ''}`}>
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
                                                    }}>Delete group</Button></th>
                                                    : ""
                                                }
                                            </tr>
                                        ))
                                    }
                                </tbody>
                                </table>
                            </div>
                        </>
                    : <p className="text-gray-400 text-5xl text-center mt-[200px] fw-bold">No students in this group</p>
                }
            </div>
        </>
    );
};

export default AdminGroup;