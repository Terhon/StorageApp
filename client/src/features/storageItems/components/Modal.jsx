export default function Modal({ title, isOpen, children }) 
{
    if (!isOpen) 
        return null;

    return (
        <dialog open className="modal">
            <div className="modal-box">
                {title && <h3 className="font-bold text-lg mb-4">{title}</h3>}

                <div>{children}</div>
            </div>
        </dialog>
    );
}
