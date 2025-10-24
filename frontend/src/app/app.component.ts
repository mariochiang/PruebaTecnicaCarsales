import { Component } from '@angular/core';
import { EpisodesListComponent } from './episodes-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [EpisodesListComponent],
  template: `<app-episodes-list />`
})
export class AppComponent {}
