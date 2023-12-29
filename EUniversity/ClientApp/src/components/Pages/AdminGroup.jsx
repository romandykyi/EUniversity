import {React, useEffect, useState} from 'react';
import { useLocation } from 'react-router-dom';
import { useAppSelector } from '../../store/store';
import Button from "../UI/Button";
import AddItemToGroupModal from '../AddItemToGroupModal';
import DeleteModal from '../DeleteModal';
import BackButton from '../UI/BackButton';
import PageForm from '../PageForm';

const AdminGroup = () => {

    const [students, setStudents] = useState([]);
    const [teacher, setTeacher] = useState({});
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [isAddStudentVisible, setIsAddStudentVisible] = useState(false);
    const [pageSize, setPageSize] = useState(10);
    const [page, setPage] = useState(1);
    const [inputValue, setInputValue] = useState("");
    const [sortingMethod, setSortingMethod] = useState(0);
    const [isEditable, setIsEditable] = useState(false);
    const [deletedUser, setDeletedUser] = useState({
        id: '',
        name: ''
    });
    const location = useLocation();
    const regex = /\/groups\/(\d+)/;
    const groupNumber = location.pathname.match(regex)[1];
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    useEffect(() => {
        getTeacher();
    }, []);

    const getTeacher = async () => {
        try {
            const response = await fetch(`/api/groups/${groupNumber}`);
            if (response.ok) {
                const data = await response.json();
                setTeacher(data.teacher);
            }
        } catch(e) {
            console.log(e);
        }
    }

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
            <div className="students container max-w-[1100px] pt-10">
                <div className="flex items-center gap-3 mb-14">
                    <BackButton navigate="groups"/>
                    <h1 className="students__title form__title mb-0">
                    Group #{groupNumber}
                    </h1>
                </div>
                <h2 className="text-3xl font-bold mb-0">
                    Teacher: {teacher.firstName} {teacher.lastName}
                </h2>
            </div>
            <PageForm
                setItems={setStudents}
                additionalComponents={
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
                        />
                    </>
                }
                additionalItems =
                    {
                        isAdmin 
                        ?   <Button onClick={() => setIsAddStudentVisible(true)}>Add student to group</Button>
                        :   ""
                    }
                
                registerTitle="students"
                tableBody={(
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
                )}
                tableHead={(
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
                )}
                usersType={isAddStudentVisible}
                searchLink={`/api/groups/${groupNumber}/students?Page=${page}&PageSize=${pageSize}&FullName=${inputValue}&SortingMode=${sortingMethod}`}
                fetchLink={`/api/groups/${groupNumber}/students?Page=${page}&PageSize=${pageSize}&SortingMode=${sortingMethod}`}
                currentPage={page}
                setCurrentPage={setPage}
                itemsPerPage={pageSize}
                setItemsPerPage={setPageSize}
                inputValue={inputValue}
                setInputValue={setInputValue}
                isDeleteVisible={isDeleteVisible}
                setSortingMethod={setSortingMethod}
                sortingMethod={sortingMethod}
                isEditVisible={isEditable}
            />   
        </>
    );
};

export default AdminGroup;