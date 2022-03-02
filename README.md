# 
Gradually, more and more fields have been added to our logging.  In all reality, it is very unlikely that these fields will ever be searched on post promotion to production.  Until anyting is done to eradicate this, these fields remain and will be indexed and ultimately consuming unnecessary disk space.

We can programmatically change all of our applications to reduce or eradicate the creating of these unnecessary fields, maybe by features switched or by environment, but this will take time will likely involved more than one developers.

To address this, outside of programmatically time consuming code changes, a far simplier and much quicker solution is needed.

Let's briefly look at the types of fields that need to be removed from our logging. We know of two types. These being:
- non-wildcard nested field (eg Properties.ApplicationData). Let's call this uc-1
- field with a variant part (eg Message-1). Let's call this uc-2

For our on-premise workloads, we're still using our ELK stack to ship logs.  We ship these logs to Logz.io.  Therefore, the ELK application where we needed to make a chagne is Logstack.  This change will be a declarative configuration change.  

Fitler for uc-1:

```yml
filter {
  mutate {
    remove_field => [
      "[Properties][ApplicationData]",       
      "[Properties][QuotesRequest]"
    ]
  }
}
```

Filter for uc-2:

```yaml
filter {
  ruby {
    code => '
      event.to_hash["Properties"].to_hash.keys.each { |v|        
        if v.is_a? String and v.start_with?("Message-")
          puts v          
          event.remove("[Properties][#{v}]")
        end
      }
    '
  }
}
```

The docker-compose manifest shares a volume across our application and logstash.  This makes it for me to demonstrate this change. I'm also uses a input file directive here, whereas in contrast, we're actually using Filebeat.  For bravity, I'm omitting Filebeat here.

To build then run:

```powershell
docker-compose up --build
```

```powershell
http://localhost:5005/ad
```

```powershell
http://localhost:5005/mess
```