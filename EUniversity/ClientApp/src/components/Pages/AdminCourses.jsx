import React from 'react';
import PageOfItems from '../PageOfItems';
import { useState } from 'react';
import { useAppSelector } from '../../store/store';
import Button from '../UI/Button';
import DeleteModal from '../UI/DeleteModal';
import AddClassroomModal from '../UI/AddClassroomModal';
import Search from '../Search';

const AdminCourse = () => {

    const [courses, setCourses] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [itemsPerPage, setItemsPerPage] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [isDeleteVisible, setIsDeleteVisible] = useState(false);
    const [isAddVisible, setIsAddVisible] = useState(false);
    const [inputValue, setInputValue] = useState("");
    const [foundCourses, setFoundCourses] = useState([]);
    const [deletedCourse, setDeletedCourse] = useState({
        id: '',
        name: ''
    });
    const isAdmin = useAppSelector(state => state.isAdmin.isAdmin);

    const fetchCourses = async(page = 1, pageSize = 10) => {

        try {
            const response = await fetch(`/api/courses?Page=${page}&PageSize=${pageSize}`);
            if (response.ok) {
                const data = await response.json();
                setCourses(data.items);
                setItemsPerPage(data.pageSize);
                setTotalItems(data.totalItemsCount);
                setIsLoading(false);

            } else {
                console.log('error');
            }
        } catch(error) {
            console.log(error);
        }
    };

    const deleteCourse = async(courseId) => {
        try {
            const response = await fetch(`/api/Courses/${courseId}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                }
            });
            if (response.ok) {
                console.log(`deleted Course: ${deletedCourse.name}`);
                fetchCourses();
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
                isVisible={isDeleteVisible}
                setIsVisible={setIsDeleteVisible}
                itemType = "course"
                deleteFunction = {deleteCourse}
                deletedUser = {deletedCourse}
            />
            <AddClassroomModal
                isVisible={isAddVisible}
                setIsVisible={setIsAddVisible}
                title="course"
                responseTitle="courses"
                fetchItems={fetchCourses}
            />
            <PageOfItems
                title = {`All Courses (${totalItems})`}
                fetchFunction = {fetchCourses}
                isLoading = {isLoading}
                itemsPerPage = {itemsPerPage}
                setItemsPerPage = {setItemsPerPage}
                totalItems = {totalItems}
                tableHead = {(
                    <tr>
                        <th>Name of the course</th>
                        {
                            isAdmin 
                            ? <th>Delete</th>
                            : ""
                        }
                    </tr>
                )}
                tableBody = {(
                    (inputValue.length ? foundCourses : courses).map((item) => (
                        <tr 
                            key={item.id}
                        >
                            <td>{item.name}</td>
                            {
                                isAdmin 
                                    ? <th><Button onClick = {() => {
                                        setIsDeleteVisible(true);
                                        setDeletedCourse({id: item.id, name: item.name});
                                    }}>Delete Course</Button></th>
                                    : ""
                            }

                        </tr>
                    ))
                )}
                additionalItems={
                    <>
                        <Search
                            inputValue={inputValue}
                            setInputValue={setInputValue}
                            setFoundItems={setFoundCourses}
                            link="/api/courses?name="
                        />
                        {
                            isAdmin 
                            ? <Button onClick={() => setIsAddVisible(true)}>Add courses</Button>
                            : ""
                        }
                    </>
                  }
            />   
        </>
    );
};

export default AdminCourse;