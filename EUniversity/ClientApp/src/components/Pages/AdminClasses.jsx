import React, { useEffect } from 'react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DeleteModal from '../DeleteModal';
import { useAppSelector } from '../../store/store';
import Button from '../UI/Button';
import PageForm from '../PageForm';
import EditFormModal from '../EditFormModal';
import SearchSelectForClasses from '../UI/SearchSelectForClasses';

const AdminClasses = () => {

    const [classes, setClasses] = useState([]);
    const [pageSize, setPageSize] = useState(10);
    const [page, setPage] = useState(1);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [sortingMethod, setSortingMethod] = useState(0);
    const [isEditable, setIsEditable] = useState(false);
    const [editableItem, setEditableItem] = useState(null);
    const [searchClass, setSearchClass] = useState({});
    const [searchLink, setSearchLink] = useState(`/api/classes?Page=${page}&PageSize=${pageSize}&name=${inputValue}&sortingMode=${sortingMethod}`);
    const [deletedClass, setDeletedClass] = useState({
        id: '',
        name: ''
    });

    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);
    const navigate = useNavigate();

    useEffect(() => {
        const teacher = searchClass.teacher ? `&TeacherId=${searchClass.teacher}` : "";
        const group = searchClass.group ? `&GroupId=${searchClass.group}` : "";
        const classroom = searchClass.classroom ? `&ClassroomId=${searchClass.classroom}` : "";
        const classType = searchClass.classType ? `&ClassTypeId=${searchClass.classType}` : "";
        const newLink = `/api/classes?Page=${page}&PageSize=${pageSize}${teacher}${group}${classroom}${classType}`;

        setSearchLink(newLink)
    }, [searchClass, page, pageSize])

    const handleInputChange = (id, field, value) => {
        const newData ={ ...searchClass, [field]: value };
        setSearchClass(newData);
    };

    const convertDateFormat = (inputDate) => {
        const dateObj = new Date(inputDate);
    
        const hours = ('0' + dateObj.getHours()).slice(-2);
        const minutes = ('0' + dateObj.getMinutes()).slice(-2);
        const day = ('0' + dateObj.getDate()).slice(-2);
        const month = ('0' + (dateObj.getMonth() + 1)).slice(-2);
        const year = dateObj.getFullYear();
    
        const formattedDate = `${hours}:${minutes} ${day}.${month}.${year}`;
    
        return formattedDate;
    }

    const deleteClass = async(classroomId) => {
        try {
            const response = await fetch(`/api/classes/${classroomId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                setIsDeleteVisible();
            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };



    return (
            <PageForm
                setItems={setClasses}
                additionalItems = {
                    <div className="flex gap-3 flex-wrap w-full">
                        <div>
                            <SearchSelectForClasses
                                handleInputChange={handleInputChange} 
                                itemId={searchClass.id}
                                link="/api/users/teachers?Page=1&PageSize=20&FullName="
                                title="teacher"
                            />
                        </div>
                        <div>
                            <SearchSelectForClasses
                                handleInputChange={handleInputChange} 
                                itemId={searchClass.id}
                                link="/api/groups?Page=1&PageSize=20&name="
                                title="group"
                            />
                        </div>
                        <div>
                            <SearchSelectForClasses
                                handleInputChange={handleInputChange} 
                                itemId={searchClass.id}
                                link="/api/classrooms?Page=1&PageSize=20&name="
                                title="classroom"
                            />
                        </div>
                       <div>
                            <SearchSelectForClasses
                                handleInputChange={handleInputChange} 
                                itemId={searchClass.id}
                                link="/api/classTypes?Page=1&PageSize=20&name="
                                title="classType"
                            />
                       </div>
                    </div>
                  }
                additionalComponents={
                    <>
                        <DeleteModal
                            isVisible={isDeleteVisible}
                            setIsVisible={setIsDeleteVisible}
                            itemType = "class"
                            deleteFunction = {deleteClass}
                            deletedItem = {deletedClass}
                        />
                       {
                        editableItem
                        ?    <EditFormModal
                                item={editableItem}
                                isEditable={isEditable}
                                setIsEditable={setIsEditable}
                                itemType="class"
                                responseTitle="classes"
                            />
                        : ""
                       }
                    </>
                }
                registerTitle="classes"
                tableBody={(
                    classes.map((item) => (
                    <tr 
                        onClick={() => {
                            navigate(`${item.id}`);
                        }} 
                        key={item.id} className="cursor-pointer"
                    >
                        <td>{item.group.name}</td>
                        <td>{convertDateFormat(item.startDate)}</td>
                        <td>{item.group.teacher.firstName} {item.group.teacher.lastName}</td>
                        <td>{item.group.course.name}</td>
                        <td>{item.duration}</td>
                        <td>{item.group.course.semester ? item.group.course.semester.name : "no semester"}</td>
                        {
                            isAdmin 
                                ?   <>
                                        <td>
                                            <Button onClick = {e => 
                                                {
                                                    e.stopPropagation();
                                                    setIsDeleteVisible(true);
                                                    setDeletedClass({id: item.id, name: item.name});
                                                }}
                                            >Delete Class</Button>
                                        </td>
                                        <td>
                                            <Button onClick = {e => 
                                                {
                                                    e.stopPropagation();
                                                    setIsEditable(true);
                                                    setEditableItem(item);
                                                }}
                                            >Edit Class</Button>
                                        </td>
                                    </>
                                :   ""
                        }
                        </tr>
                    ))
                )}
                tableHead={(
                    <tr>
                        <th>Group</th>
                        <th>Start</th>
                        <th>Teacher</th>
                        <th>Course</th>
                        <th>Duration</th>
                        <th>Semester</th>
                        {
                            isAdmin 
                            ?   <>
                                    <th>Delete</th>
                                    <th>Edit</th>
                                </>
                            :   ""
                        }
                    </tr>
                )}
                searchLink={`/api/classes?Page=${page}&PageSize=${pageSize}&name=${inputValue}&sortingMode=${sortingMethod}`}
                fetchLink={searchLink}
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

export default AdminClasses;