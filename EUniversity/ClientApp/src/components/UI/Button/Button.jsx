

const Button = ({ children, ...props }) => {
    return (
        <button className="px-3 py-2 text-white text-xl bg-sky-500 rounded-lg outline-none border-none transition-transform duration-200 cursor-pointer hover:bg-blue-600 active:transform-active disabled:bg-gray-500 disabled:cursor-not-allowed disabled:active:transform-none" {...props}>
            {children}
        </button>
    )
}

export default Button;