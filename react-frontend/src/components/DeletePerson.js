import React, { Component } from "react";
import ReactDOM from "react-dom";
import PeopleDisplay from "./PeopleDisplay";
import PeopleTableHeader from "./PeopleTableHeader";
import PeopleTableBody from "./PeopleTableBody";

class DeletePerson extends Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      isDeleted: false
    };
  }

  componentDidMount() {
    const URL = "/api/react/" + this.props.id;
    fetch(URL, { method: 'DELETE' })
    .then(
      (result) => {
        this.setState({
          isDeleted: true
        });
      },
      (error) => {
        this.setState({
          isDeleted: true,
          error
        })
      },
    )
  }

  render() {
    const {error, isDeleted} = this.state;
    if (error) {
      return <div>Could not delete person!</div>
    } else if (!isDeleted) {
      return <div>Deleting ...</div>
    } else {
      ReactDOM.unmountComponentAtNode(document.getElementById('List'));
      ReactDOM.render(<PeopleDisplay />, document.getElementById('List'));
      ReactDOM.unmountComponentAtNode(document.getElementById('Table'));
      ReactDOM.render(<table><PeopleTableHeader /><PeopleTableBody /></table>,
       document.getElementById('Table'));
      return (
        <article className="delete">
          <h2>Person is deleted!</h2>
        </article>
      )
    }
  }

};

export default DeletePerson;

