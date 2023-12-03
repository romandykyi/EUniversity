import React from 'react';
import { useState } from 'react';
import { useAppSelector } from '../../store/store';
import Button from '../UI/Button';
import DeleteModal from '../DeleteModal';
import PageForm from '../PageForm';
import EditFormModal from '../EditFormModal';

const AdminCourse = () => {

    const [courses, setCourses] = useState([]);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [pageSize, setPageSize] = useState(10);
    const [page, setPage] = useState(1);
    const [sortingMethod, setSortingMethod] = useState(0);
    const [isEditable, setIsEditable] = useState(false);
    const [editableItem, setEditableItem] = useState(null);
    const [deletedCourse, setDeletedCourse] = useState({
        id: '',
        name: ''
    });
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    const deleteCourse = async(courseId) => {
        try {
            const response = await fetch(`/api/Courses/${courseId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                setIsDeleteVisible(false);
            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };

    return (
         <PageForm
                setItems={setCourses}
                additionalComponents={
                   <>
                        <DeleteModal
                            isVisible={isDeleteVisible}
                            setIsVisible={setIsDeleteVisible}
                            itemType = "course"
                            deleteFunction = {deleteCourse}
                            deletedItem = {deletedCourse}
                        />
                        {
                            editableItem
                            ?    <EditFormModal
                                    item={editableItem}
                                    isEditable={isEditable}
                                    setIsEditable={setIsEditable}
                                    itemType="course"
                                    responseTitle="courses"
                                />
                            : ""
                        }
                   </>
                }
                registerTitle="courses"
                tableBody={(
                    courses.map((item) => (
                        <tr 
                            key={item.id}
                        >
                            <td>{item.name}</td>
                            {
                                isAdmin 
                                    ?   <>
                                            <td>
                                                <Button onClick = {() => 
                                                    {
                                                        setIsDeleteVisible(true);
                                                        setDeletedCourse({id: item.id, name: item.name});
                                                    }}
                                                >Delete Course</Button>
                                            </td>
                                            <td>
                                                <Button onClick = {e => 
                                                    {
                                                        e.stopPropagation();
                                                        setIsEditable(true);
                                                        setEditableItem(item);
                                                    }}
                                                >Edit Course</Button>
                                            </td>
                                        </>
                                    : ""
                            }

                        </tr>
                    ))
                )}
                tableHead={(
                    <tr>
                        <th>Name of the course</th>
                        {
                            isAdmin 
                            ?   <>
                                    <th>Delete</th>
                                    <th>Edit</th>
                                </>
                            : ""
                        }
                    </tr>
                )}
                searchLink={`/api/courses?Page=${page}&PageSize=${pageSize}&name=${inputValue}&sortingMode=${sortingMethod}`}
                fetchLink={`/api/courses?Page=${page}&PageSize=${pageSize}&sortingMode=${sortingMethod}`}
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
    );
};

export default AdminCourse;