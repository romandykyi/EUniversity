import React, { useRef } from 'react';
import Button from './UI/Button';
import { useState, useEffect } from 'react';
import SearchSelect from './UI/SearchSelect';
import { useAppSelector } from '../store/store';
import SearchSelectForClasses from './UI/SearchSelectForClasses';

const EditFormModal = ({ 
    item, 
    isEditable, 
    setIsEditable,
    responseTitle
}) => {
    const [editedItem, setEditedItem] = useState(item);
    const [itemParams, setItemParams] = useState({});
    const [tableHead, setTableHead] = useState();
    const [tableBody, setTableBody] = useState();
    const [errors, setErrors] = useState([]);
    const durationRef = useRef(null);
    const startDateRef = useRef(null);
    const startTimeRef = useRef(null);
    const semesterDateFromRef = useRef(null);
    const semesterDateToRef = useRef(null);
    const isThemeDark = useAppSelector(state => state.theme.isThemeDark);

    useEffect(() => {
        if (durationRef.current) durationRef.current.defaultValue = editedItem.duration.slice(0,-3);
        if (startDateRef.current) startDateRef.current.defaultValue = convertTimeFormat(editedItem.startDate, "date");
        if (startTimeRef.current) startTimeRef.current.defaultValue = convertTimeFormat(editedItem.startDate, "time");
        if(semesterDateFromRef.current) semesterDateFromRef.current.defaultValue = convertTimeFormat(editedItem.dateFrom, "date");
        if(semesterDateToRef.current) semesterDateToRef.current.defaultValue = convertTimeFormat(editedItem.dateTo, "date");
    });

    useEffect(() => {
        setEditedItem(item);
    }, [item]);

    useEffect(() => {
        switch(responseTitle) {
            case "users":
                setTableHead((
                        <tr>
                            <th>Email</th>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Middle Name</th>
                            <th>Role</th>
                            <th>Delete</th>
                        </tr>
                    ));
                    setTableBody((
                        <tr>
                            <td>
                                    <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                        type="text"
                                        placeholder="email"
                                        value={editedItem.email}
                                        onChange={(e) => handleInputChange(editedItem.id,'email', e.target.value)}
                                    />

                            </td>
                            <td>
                                    <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                        type="text"
                                        placeholder="first name"
                                        value={editedItem.firstName}
                                        onChange={(e) => handleInputChange(editedItem.id,'firstName', e.target.value)}
                                    />

                            </td>
                            <td>
                                    <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                        type="text"
                                        value={editedItem.lastName}
                                        placeholder="last name"
                                        onChange={(e) => handleInputChange(editedItem.id,'lastName', e.target.value)}
                                    />

                            </td>
                            <td>
                                    <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                        type="text"
                                        placeholder="middle name"
                                        value={editedItem.middleName}
                                        onChange={(e) => handleInputChange(editedItem.id,'middleName', e.target.value)}
                                    />

                            </td>
                            <td> 
                                <select 
                                    className="form-select w-40 text-text bg-background" 
                                    onChange={(e) => handleInputChange(editedItem.id,'role', e.target.value)}
                                >
                                    <option value="student">student</option>
                                    <option value="teacher">teacher</option>
                                </select>
                            </td>
                        </tr>
                    ));
                setItemParams({
                    email:'',
                    firstName:'',
                    lastName:'',
                    middleName:''
                });
                break;
            case "groups":
                setTableHead((
                        <tr>
                            <th>Name</th>
                            <th>Teacher</th>
                            <th>Course</th>
                        </tr>
                    ));
                    setTableBody((
                        <tr>
                            <td>
                                <input
                                className="text-text bg-background form-control focus:text-text focus:bg-background"
                                    type="text"
                                    placeholder="name"
                                    value={editedItem.name}
                                    onChange={(e) => handleInputChange(editedItem.id, 'name', e.target.value)}
                                />
                            </td>
                            <td>
                                <SearchSelect
                                    handleInputChange={handleInputChange}
                                    itemId={editedItem.id}
                                    link="/api/users/teachers?Page=1&PageSize=20&FullName="
                                    title="teachers"
                                    givenValue={`${editedItem.teacher.firstName} ${editedItem.teacher.lastName}`}
                                />
                            </td>
                            <td>
                                <SearchSelect
                                    handleInputChange={handleInputChange}
                                    itemId={editedItem.id}
                                    link="/api/courses?name="
                                    title="courses"
                                    givenValue={editedItem.course.name}
                                />
                            </td>
                        </tr>
                    ));
                setItemParams({
                    name: '',
                    course: 0,
                    teacher: ''
                });
                break;
            case "semesters":
                setTableHead((
                        <tr>
                            <th>Name</th>
                            <th>From</th>
                            <th>To</th>
                        </tr>
                    ));
                    setTableBody((
                        <tr>
                            <td>
                                <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                    type="text"
                                    placeholder="search"
                                    value={editedItem.name}
                                    onChange={(e) => handleInputChange(editedItem.id,'name', e.target.value)}
                                />
    
                            </td>
                            <td>
                                <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                    type="date"
                                    placeholder="from"
                                    ref={semesterDateFromRef}
                                    onChange={(e) => handleInputChange(editedItem.id,'dateFrom', e.target.value)}
                                />
                            </td>
                            <td>
                            <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                    type="date"
                                    placeholder="to"
                                    ref={semesterDateToRef}
                                    onChange={(e) => handleInputChange(editedItem.id,'dateTo', e.target.value)}
                                />
                            </td>
                        </tr>
                    ));
                setItemParams({
                    name: '',
                    dateFrom: '',
                    dateTo: ''
                });
                break;
            case "classes":
                    setTableHead((
                            <tr>
                                <th>Class type</th>
                                <th>Classroom</th>
                                <th>Group</th>
                                <th>Start date</th>
                                <th>Start time</th>
                                <th>Duration</th>
                            </tr>
                        ));
                        setTableBody(
                            <tr>
                                <td>
                                    <SearchSelectForClasses
                                        handleInputChange={handleInputChange}
                                        itemId={editedItem.id}
                                        link="/api/classTypes?name="
                                        title="classType"
                                        givenValue={editedItem.classType.name}
                                    />
                                </td>
                                <td>
                                    <SearchSelectForClasses
                                        handleInputChange={handleInputChange}
                                        itemId={editedItem.id}
                                        link="/api/classrooms?name="
                                        title="classroom"
                                        givenValue={editedItem.classroom.name}
                                    />
                                </td>
                                <td>
                                    <SearchSelectForClasses
                                        handleInputChange={handleInputChange}
                                        itemId={editedItem.id}
                                        link="/api/groups?name="
                                        title="group"
                                        givenValue={editedItem.group.name}
                                    />
                                </td>
                                <td>
                                    <input
                                        className="bg-background text-text"
                                        type="date"
                                        placeholder="from"
                                        ref = {startDateRef}
                                        onChange={(e) => handleInputChange(editedItem.id, 'startDate', e.target.value)}
                                    />
                                </td>
                                <td>
                                    <input 
                                        type="time"
                                        onChange={(e) => handleInputChange(editedItem.id, 'startTime', e.target.value)}
                                        ref={startTimeRef}
                                    />
                                </td>
                                <td>
                                    <input 
                                        ref={durationRef}
                                        className="bg-background" 
                                        type="text" 
                                        pattern="[0-2][0-9]:[0-5][0-9]"
                                        onChange={(e) => handleInputChange(editedItem.id, 'duration', e.target.value)} 
                                        placeholder="example: 01:45"
                                    />
                                </td>
                            </tr>
                        );
                    setItemParams({
                        name: ''
                    });
                    break;
            default:
                setTableHead((
                        <tr>
                            <th>Name</th>
                        </tr>
                    ));
                    setTableBody((
                        <tr>
                            <td>
                                    <input
                                    className="text-text bg-background form-control focus:text-text focus:bg-background"
                                        type="text"
                                        placeholder="name"
                                        value={editedItem.name}
                                        onChange={(e) => handleInputChange(editedItem.id, 'name', e.target.value)}
                                    />
    
                            </td>
                        </tr>
                    ));
                setItemParams({
                    name: ''
                });
                break;
        }
    }, [editedItem]);
    
    const putItems = async() => {
        let postItem = {};

        if (responseTitle === "groups") {
            postItem = {
                name: editedItem.name, 
                courseId: typeof editedItem.course !== 'object' ? editedItem.course : editedItem.course.id,
                teacherId: typeof editedItem.teacher !== 'object' ? editedItem.teacher : editedItem.teacher.id
            };
        }
        else if (responseTitle === "semesters") {
            postItem = {
                name: editedItem.name,
                dateFrom: editedItem.dateFrom,
                dateTo: editedItem.dateTo,
            };
        }
        else if (responseTitle === "classes") {
            postItem = {
                classTypeId: typeof editedItem.classType !== 'object' ? editedItem.classType : editedItem.classType.id,
                classroomId: typeof editedItem.classroom !== 'object' ? editedItem.classroom : editedItem.classroom.id,
                groupId: typeof editedItem.group !== 'object' ? editedItem.group : editedItem.group.id,
                startDate: convertDateFormat(`${editedItem.startTime ? ` ${editedItem.startTime} ${editedItem.startDate}` : editedItem.startDate}`),
                duration: editedItem.duration.length < 8 ? `${editedItem.duration}:00` : editedItem.duration
            };  
            console.log(postItem)
        }
        else {
            postItem = {
                name: editedItem.name
            };
        }
        try {
            const response = await fetch(`/api/${responseTitle}/${editedItem.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: `${JSON.stringify(postItem)}`
            });

            if (response.ok) {
                setIsEditable(false);
                setErrors([]);
            }
            else {
                const responseBody = await response.json();
                    const errors = responseBody.errors;
                    let errorsArray = [];
                    for (let key in errors) {
                        const value = errors[key];
                        for (let er of value) {
                            errorsArray.push(er);
                        }
                    }
                    setErrors(errorsArray);
            }
        } catch(e) {
            console.log(e);
        }
    };
    
    const handleClickOnBg = () => {
        setIsEditable(false);
        setErrors([]);
        document.body.style.overflow = 'auto';
    };

    const handleInputChange = (id, field, value) => {
        const changedItem = { ...editedItem, [field]: value };
        setEditedItem(changedItem);
    };

    const convertTimeFormat = (inputTime, type) => {
        const date = new Date(inputTime);
        const day = date.getDate().toString().padStart(2, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear();
        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');


        if (type === "date") return `${year}-${month}-${day}`;
        return `${hours}:${minutes}`;
    };

    const convertDateFormat = (inputDateString) => {
        const inputDate = new Date(inputDateString);
    
        const year = inputDate.getUTCFullYear();
        const month = inputDate.getUTCMonth() + 1;
        const day = inputDate.getUTCDate();
        const hours = inputDate.getUTCHours();
        const minutes = inputDate.getUTCMinutes();
        const seconds = inputDate.getUTCSeconds();
        const milliseconds = inputDate.getUTCMilliseconds();
    
        const outputDateString = `${year}-${month < 10 ? '0' : ''}${month}-${day < 10 ? '0' : ''}${day}T${hours < 10 ? '0' : ''}${hours}:${minutes < 10 ? '0' : ''}${minutes}:${seconds < 10 ? '0' : ''}${seconds}.${milliseconds}Z`;
    
        return outputDateString;
    }

    return (
        <div 
            onClick={handleClickOnBg}
            className={`${isEditable ? "fixed" : "hidden"} top-0 bottom-0 left-0 right-0 bg-black bg-opacity-50 z-30 flex items-center justify-center px-4`}
        >
            <div 
                className=" container max-w-[1100px] bg-background p-4 rounded-lg flex items-center justify-center flex-col text-center" 
                onClick={(e) => e.stopPropagation()}
            >
                <div className="flex items-center justify-between w-full">
                    <h1 className="newUser__title form__title font-medium mb-0">
                        Editing {item.name}
                    </h1>
                    <Button onClick={() => {
                        setIsEditable(false);
                        
                        setErrors([]);
                    }} addStyles="bg-danger">Cancel</Button>
                </div>
                <div className="table-container w-full">
                    <table className={`table w-full table-hover ${isThemeDark ? 'table-dark' : ''}`}>
                        <thead>
                            {tableHead}
                        </thead>
                        <tbody>
                            {tableBody}
                        </tbody>
                    </table>
                </div>
                <div className="newUser__error form__error">
                    {
                        errors.map(error => <><p key={error}>{error}</p></>)
                    }
                </div>
                <Button onClick={putItems}>Save Changes</Button>
            </div>
        </div>
    );
};

export default EditFormModal;