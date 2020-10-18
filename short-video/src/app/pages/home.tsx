import React, { Component } from "react";
import { NeuCard } from "../../components/neu-components/cards/neu-card";
import { NeuColumn } from "../../components/neu-components/collections/neu-column";
import { NeuGrid } from "../../components/neu-components/collections/neu-grid";
import { NeuRow } from "../../components/neu-components/collections/neu-row";
import "../../components/neu-components/styles/standard.scss";

export default class Home extends Component {
  constructor(props) {
    super(props);
  }
  token = localStorage.getItem('token')
  description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Recusandae, sint culpa reiciendis ut amet doloremque ad numquam, optio commodi aspernatur asperiores porro autem rerum debitis impedit deserunt nesciunt inventore ea.";

  render() {

    return (
      <NeuGrid>
        <NeuRow columnsCount={2} >
          <NeuColumn >
            <NeuCard title='Data' cardImageUrl='https://dll.mmcee.cn/78717395_p0.png' description={this.description}></NeuCard>
          </NeuColumn>
        </NeuRow>
      </NeuGrid>
    );
  }
}
