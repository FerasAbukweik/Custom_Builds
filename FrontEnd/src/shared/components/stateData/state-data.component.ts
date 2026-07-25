import { Component, input } from '@angular/core';
import { stateCardData } from '../../../core/interfaces/state-card-data';

@Component({
  selector: 'app-state-data',
  imports: [],
  templateUrl: './state-data.component.html',
  host: {
    class: 'flex flex-wrap gap-6',
  },
})
export class StateDataComponent {
  // input
  statCardData = input.required<stateCardData>();
}
