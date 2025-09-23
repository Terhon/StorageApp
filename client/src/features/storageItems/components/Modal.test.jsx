import { render, screen } from "@testing-library/react";
import Modal from "./Modal";

describe("Modal", () => {
    it("does not render when isOpen is false", () => {
        const { container } = render(<Modal title="Test Modal" isOpen={false} />);
        expect(container).toBeEmptyDOMElement();
    });

    it("renders title and children when open", () => {
        render(
            <Modal title="My Modal" isOpen={true}>
                <p>Modal content</p>
            </Modal>
        );

        expect(screen.getByText("My Modal")).toBeInTheDocument();
        expect(screen.getByText("Modal content")).toBeInTheDocument();
    });

    it("renders without title", () => {
        render(
            <Modal isOpen={true}>
                <span>No title</span>
            </Modal>
        );

        expect(screen.queryByRole("heading")).not.toBeInTheDocument();
        expect(screen.getByText("No title")).toBeInTheDocument();
    });
});
