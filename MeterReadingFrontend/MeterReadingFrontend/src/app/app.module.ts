import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { MeterReadingUploadComponent } from './components/meter-reading-upload/meter-reading-upload.component';
import { MeterReadingService } from './services/meter-reading.service';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { ResponseDisplayComponent } from './components/response-display/response-display.component';

@NgModule({
  declarations: [AppComponent, MeterReadingUploadComponent, ResponseDisplayComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule, RouterModule, AppRoutingModule],
  providers: [MeterReadingService],
  bootstrap: [AppComponent],
})
export class AppModule {}
