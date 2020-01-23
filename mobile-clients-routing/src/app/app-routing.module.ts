import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { LineupComponent } from './lineup/lineup.component';
import { RacesComponent } from './races/races.component';


const routes: Routes = [
  { path: 'lineup', component: LineupComponent},
  { path: 'tournament/:id', component: RacesComponent},
  { path: '', component: RacesComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
