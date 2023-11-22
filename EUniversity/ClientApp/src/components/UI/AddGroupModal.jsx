import { useState } from "react";
import Table from "./Table/Table";
import Button from "./Button";
import SearchSelect from "./SearchSelect";

const AddGroupModal = ({
    isVisible,
    setIsVisible,
    title,
    responseTitle,
    fetchItems
}) => {

    const [error,setError] = useState('');
    const [items, setItems] = useState([]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        const postItems = items.map(item => ({
            name: item.name,
            courseId: item.course,
            teacherId: item.teacher
        }));
        for (let postItem of postItems) {
            try {
                const response = await fetch(`/api/${responseTitle}`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: `${JSON.stringify(postItem)}`,
                });
    
                if (response.ok) {
                    console.log('ok');
                    setIsVisible(false);
                    document.body.style.overflow = 'auto';
                    setItems([]);
                    setError('');
                    await fetchItems();
                } 
                else {
                    console.error("Error:", response.status, response.statusText);
                    setError(`${response.status} ${response.statusText}`);
                }
            } catch (error) {
                console.error("An error occurred:", error);
                setError('An error occurred while adding new classroom.');
            }
        }
    };

    const handleClickOnBg = () => {
        setIsVisible(false);
        document.body.style.overflow = 'auto';
    };

    const handleInputChange = (id, field, value) => {
        const newData = items.map((row) =>
            row.id === id ? { ...row, [field]: value } : row
        );
        setItems(newData);
    };

    const deleteItem = (id) => {

        const newData = items.filter(item => item.id !== id);
        setItems(newData);
    }

    return (
        <div 
            onClick={handleClickOnBg}
            className={`${isVisible ? "fixed" : "hidden"} top-0 bottom-0 left-0 right-0 bg-black bg-opacity-50 z-30 flex items-center justify-center px-4`}
        >
            <div 
                className=" container max-w-[1100px] pt-10 bg-white p-10 rounded-lg" 
                onClick={(e) => e.stopPropagation()}
            >
                <h1 className="newUser__title form__title">
                    Register new {title}
                </h1>
                <form onSubmit={handleSubmit} className="newUser form__form">
                        <Table
                            items={items}
                            setItems={setItems}
                            title={title}
                            itemParams={{
                                name: '',
                                course: '',
                                teacher: ''
                            }}
                            tableHead={(
                                <tr>
                                    <th>Name</th>
                                    <th>Teacher</th>
                                    <th>Course</th>
                                    <th>Delete</th>
                                </tr>
                            )}
                            tableBody={items.map((row) => (
                                <tr key={row.id}>
                                    <td>
                                        <input
                                            type="text"
                                            placeholder="search"
                                            value={row.name}
                                            onChange={(e) => handleInputChange(row.id, 'name', e.target.value)}
                                        />
            
                                    </td>
                                    <td>
                                        <SearchSelect
                                            handleInputChange={handleInputChange}
                                            itemId={row.id}
                                            link="/api/users/teachers?Page=1&PageSize=20&FullName="
                                            title="teachers"
                                        />
                                    </td>
                                    <td>
                                        <SearchSelect
                                            handleInputChange={handleInputChange}
                                            itemId={row.id}
                                            link="/api/courses?name="
                                            title="courses"
                                        />
                                    </td>
                                    <td>
                                        <Button onClick = {e => {
                                            e.preventDefault();
                                            deleteItem(row.id);
                                        }}>Delete</Button>
                                    </td>
                                </tr>
                            ))}
                        />
                        <div className="newUser__error form__error">
                            {error}
                        </div>
                    <Button type="submit">Register new {title}</Button>
                </form>
            </div>
        </div>
    );
};

export default AddGroupModal;