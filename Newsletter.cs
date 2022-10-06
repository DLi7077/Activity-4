using System;


namespace Newsletter
{
  interface ArticleStuff
  {
    public List<String>? articles { get; set; }
  }

  interface User
  {
    public string? username { get; set; }
    public bool subscribed { get; set; }
  }

  class BlogPost
  {
    public int articleCount { get; set; } = 0;
    private List<string> articles = new List<string>();

    public string this[int articleIdx]
    {
      get { return articles[articleIdx]; }
      set { articles[articleIdx] = value; }
    }

    public event Action<string>? OnPublish;
    public void publish(string article)
    {
      if (OnPublish != null)
      {
        articles.Add(article);
        Console.WriteLine($"Article {articleCount++}");
        OnPublish(article);
      }
    }

    public bool EditArticle(int articleIdx, string articleContent)
    {
      if (articleIdx >= articles.Count || articleIdx < 0) return false;
      articles[articleIdx] = articleContent;
      return true;
    }

  }

  class Member : User
  {
    public Member(string username)
    {
      this.username = username;
    }
    public string? username { get; set; }
    public bool subscribed { get; set; } = false;
    private void ReadArticle(string article)
    {
      subscribed = true;
      Console.WriteLine($"{username} is reading:\n\t{article}");
    }

    public Action<string>? handleSubscribe()
    {
      if (subscribed)
      {
        Console.WriteLine("already subscribed");
        return null; // is this an issue?
      }
      subscribed = true;
      return ReadArticle;
    }
    public Action<string>? handleUnsubscribe()
    {
      if (!subscribed)
      {
        Console.WriteLine("already unsubscribed");
        return null; // is this an issue?
      }
      subscribed = false;
      return ReadArticle;
    }
  }

  class Program
  {
    public static void Main()
    {
      BlogPost Recipes = new BlogPost();
      Member Obama = new Member(username: "Obama");
      Member Trump = new Member(username: "Trump");
      Member Jesus = new Member(username: "Jesus");

      Recipes.OnPublish += Obama.handleSubscribe();

      Recipes.publish("Today, we will be making a bacon egg and cheese sandwich for my boy youssef, the Ocky way");

      Recipes.OnPublish += Trump.handleSubscribe();
      Recipes.publish("Today, we will be baking a carrot cake with butter chicken");
      Recipes.OnPublish -= Trump.handleUnsubscribe();
      Recipes.publish("Today, we will make fillet mignon with a twist: Grilled Cheese");
    }
  }

}