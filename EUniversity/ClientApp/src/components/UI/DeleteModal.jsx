import React from 'react';
import Button from './Button';

const DeleteModal = ({ 
    isVisible, 
    setIsVisible,
    itemType,
    deleteFunction,
    deletedItem
}) => {

    const handleClickOnBg = () => {
        setIsVisible(false);
        document.body.style.overflow = 'auto';
    };

    return (
        <div 
        onClick={handleClickOnBg}
        className={`${isVisible ? "absolute" : "hidden"} top-0 bottom-0 left-0 right-0 bg-black bg-opacity-50 z-30 flex items-center justify-center px-4`}
        >
            <div 
                className=" container max-w-[500px] bg-white p-4 rounded-lg flex items-center justify-center flex-col text-center" 
                onClick={(e) => e.stopPropagation()}
            >
                <h1 className="newUser__title form__title font-medium">
                    Delete {itemType}: {deletedItem.name}?
                </h1>
                <div className="flex items-center gap-5">
                    <Button onClick={() => deleteFunction(deletedItem.id)} className="bg-red-500 px-3 py-2 font-medium text-white text-xl rounded-lg outline-none border-none transition-transform duration-200 cursor-pointer hover:bg-red-600 active:transform-active disabled:bg-gray-500 disabled:cursor-not-allowed disabled:active:transform-none">Delete</Button>
                    <Button onClick={() => setIsVisible(false)}>Cancel</Button>
                </div>
            </div>
        </div>
    );
};

export default DeleteModal;