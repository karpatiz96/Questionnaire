import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';

const routes: Routes = [
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'counter', component: CounterComponent },
    { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
    { path: 'group', loadChildren: () => import('./features/group/group.module').then(m => m.GroupModule), canActivate: [AuthorizeGuard] },
    { path: 'invitation',
      loadChildren: () => import('./features/invitation/invitation.module').then(m => m.InvitationModule), canActivate: [AuthorizeGuard] },
    { path: 'questionnaire',
      loadChildren: () => import('./features/questionnaire/questionnaire.module')
        .then(m => m.QuestionnaireModule), canActivate: [AuthorizeGuard] }
];

@NgModule({
    declarations: [],
    imports: [
      CommonModule,
      RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
  })
  export class AppRoutingModule { }
