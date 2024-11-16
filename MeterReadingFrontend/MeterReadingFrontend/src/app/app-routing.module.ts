import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MeterReadingUploadComponent } from './components/meter-reading-upload/meter-reading-upload.component';

const routes: Routes = [
  { path: 'upload', component: MeterReadingUploadComponent },
  { path: '', redirectTo: '/upload', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
