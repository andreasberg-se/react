import React, { Component } from "react";
import { ShowPerson } from "./ShowPerson";
import { PeopleSection } from "./StyledComponents";

class PeopleDisplay extends Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      isLoaded: false,
      people: []
    };
  }

  componentDidMount() {
    fetch("/api/react")
    .then(response => response.json())
    .then(
      (result) => {
        this.setState({
          isLoaded: true,
          people: result
        });
      },
      (error) => {
        this.setState({
          isLoaded: true,
          error
        })
      },
    )
  }

  render() {
    const {error, isLoaded, people} = this.state;
    if (error) {
      return <div>Loading error!</div>
    } else if (!isLoaded) {
      return <div>Loading ...</div>
    } else {
      const showPeople = () => {
        return (
          people.map((person) => <ShowPerson key={person.PersonId} item={person} />)
        )
      };
      return (
        
        <PeopleSection>{showPeople()}</PeopleSection>
      )
    }
  }

};

export default PeopleDisplay;

