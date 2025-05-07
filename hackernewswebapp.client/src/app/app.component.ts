import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatTableDataSource} from '@angular/material/table'
import { Observable } from 'rxjs';

interface HackerNewsStory {
  title: string;
  by: string;
  url: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public hackerNewsStories: HackerNewsStory[] = [];

  @ViewChild(MatPaginator) paginator: MatPaginator | null = null;
  obs: Observable<any> | undefined;
  dataSource: MatTableDataSource<HackerNewsStory> = new MatTableDataSource<HackerNewsStory>(this.hackerNewsStories);
  displayedColumns: string[] = ['sno', 'title', 'created_by', 'url'];

  constructor(private http: HttpClient, private changeDetectorRef: ChangeDetectorRef) {}

  ngOnInit() {
    this.changeDetectorRef.detectChanges();
    this.get("");
    // this.getForecasts();
  }

  ngOnDestroy() {
    if (this.dataSource) { 
      this.dataSource.disconnect(); 
    }
  }

  get(searchTerm: string) {

    var url = `/hackernews?searchTerm=${searchTerm}`;
    this.http
      .get<HackerNewsStory[]>(
        url
      )
      .subscribe(
        result => {
          this.hackerNewsStories = result;
          this.dataSource.data = this.hackerNewsStories;
          setTimeout(() => {
            //this.dataSource.paginator = this.paginator;
            this.obs = this.dataSource.connect();
          });
        },
        error => console.error(error)
      );
  }
  
  search(event: KeyboardEvent) {
    this.get((event.target as HTMLTextAreaElement).value);
  }
  
  open(url: string) {
    window.open(url, "_blank");
  }

  ngAfterViewInit() {
    this.dataSource.data = this.hackerNewsStories;
    this.dataSource.paginator = this.paginator;
  }

  title = 'hackernewswebapp.client';
}
