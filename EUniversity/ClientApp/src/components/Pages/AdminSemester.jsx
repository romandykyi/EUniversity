import {React, useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from '../../store/store';
import Button from "../UI/Button";
import AddItemToGroupModal from '../AddItemToGroupModal';
import DeleteModal from '../DeleteModal';

const AdminGroup = () => {


    const [students, setStudents] = useState([]);
    const [date, setDate] = useState({});
    const [isLoading, setIsLoading] = useState(true);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [isAddStudentVisible, setIsAddStudentVisible] = useState(false);
    const [deletedUser, setDeletedUser] = useState({
        id: '',
        name: ''
    });
    const location = useLocation();
    const regex = /\/semesters\/(\d+)/;
    const semesterNumber = location.pathname.match(regex)[1];
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    useEffect(() => {
        fetchGroup();
    }, []);

    const formatDate = (inputDate) => {
        const dateObject = new Date(inputDate);
      
        const day = String(dateObject.getDate()).padStart(2, '0');
        const month = String(dateObject.getMonth() + 1).padStart(2, '0');
        const year = dateObject.getFullYear();
      
        return `${day}.${month}.${year}`;
      }

    const fetchGroup = async(page = 1, pageSize = 10) => {
        try {
            const response = await fetch(`/api/semesters/${semesterNumber}`);
            if (response.ok) {
                const data = await response.json();
                console.log(data.studentEnrollments);
                setStudents(data.studentEnrollments);
                setDate({
                    from: formatDate(data.dateFrom),
                    to: formatDate(data.dateTo)
                });
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
            const response = await fetch(`/api/semesters/${semesterNumber}/students/${userId}`, {
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
                fetchTitle="semesters"
                groupId={semesterNumber}
                fetchItems={fetchGroup}
            />
            <div className="students container max-w-[1100px] pt-10">
                <h1 className="students__title form__title">
                    Semester #{semesterNumber}
                </h1>
                <h2 className="text-3xl font-bold mb-5">
                    From {date.from} to {date.to}
                </h2>
                {
                    isAdmin 
                    ?   <Button onClick={() => setIsAddStudentVisible(true)}>Add student to semester</Button>
                    :   ""
                }
                {
                    students.length
                    ?   <>
                            <div className="table-container mt-5">
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
                                            <tr key={item.student.id}>
                                                <td>{item.student.firstName}</td>
                                                <td>{item.student.lastName}</td>
                                                <td>{item.student.userName}</td>
                                                {
                                                isAdmin 
                                                    ? <th><Button onClick={() => {
                                                        setIsDeleteVisible(true);
                                                        setDeletedUser({id: item.student.id, name: `${item.student.firstName} ${item.student.lastName}`});
                                                    }}>Delete student</Button></th>
                                                    : ""
                                                }
                                            </tr>
                                        ))
                                    }
                                </tbody>
                                </table>
                            </div>
                        </>
                    :   <p className="text-gray-400 text-5xl text-center mt-[200px] fw-bold">No students in this semester</p>
                }
            </div>
        </>
    );
};

export default AdminGroup;