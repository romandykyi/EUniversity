import React from 'react';



const Loader = () => {
    return (
    <div className="absolute left-0 bottom-0 right-0 top-0 flex items-center justify-center">
        <div className='flex w-28 h-28 border-t-theme animate-spin rounded-full border-8'></div>
    </div>

    );
};

export default Loader;